namespace Engine.Infrastructure.AiAdapters;

using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Engine.Application.Enums;
using Engine.Application.Interfaces;
using Engine.Application.Models;
using Engine.Infrastructure.Options;

using Microsoft.Extensions.Options;

/// <summary>
/// Адаптер AI-сервиса gen-api через job-эндпоинт networks/{model}
/// (api.gen-api.ru), запросы выполняются синхронно (is_sync)
/// </summary>
public sealed class OpenRouterAdapter : IAiClient
{
    /// <summary>
    /// Параметры сериализации: не отправляем null-поля (temperature, max_tokens, name)
    /// </summary>
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// HTTP-клиент (BaseAddress и авторизация настроены при регистрации)
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Настройки адаптера
    /// </summary>
    private readonly OpenRouterOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRouterAdapter"/> class.
    /// Адаптер AI-сервиса gen-api
    /// </summary>
    /// <param name="httpClient">
    /// Типизированный HTTP-клиент
    /// </param>
    /// <param name="options">
    /// Настройки адаптера
    /// </param>
    public OpenRouterAdapter(HttpClient httpClient, IOptions<OpenRouterOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        this.httpClient = httpClient;
        this.options = options.Value;
    }

    /// <inheritdoc/>
    public async Task<string> CompleteAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payload = this.BuildPayload(request);

        // gen-api исполняет запрос синхронно и сразу возвращает результат,
        // иначе ответом был бы идентификатор задачи, который надо опрашивать отдельно
        payload.IsSync = true;

        using var response = await this.httpClient.PostAsJsonAsync(
                                 $"networks/{payload.Model}",
                                 payload,
                                 SerializerOptions,
                                 cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"gen-api вернул {(int)response.StatusCode} ({response.StatusCode}): {body}");
        }

        return ExtractText(body);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> StreamAsync(
        AiRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payload = this.BuildPayload(request);
        payload.Stream = true;

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"networks/{payload.Model}")
        {
            Content = JsonContent.Create(payload, options: SerializerOptions),
        };

        // ResponseHeadersRead — не буферизуем тело, читаем поток по мере поступления
        using var response = await this.httpClient.SendAsync(
                                 httpRequest,
                                 HttpCompletionOption.ResponseHeadersRead,
                                 cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"gen-api вернул {(int)response.StatusCode} ({response.StatusCode}): {error}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        // Server-Sent Events: строки вида "data: {json}", завершается "data: [DONE]"
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if (!line.StartsWith("data:", StringComparison.Ordinal))
            {
                continue;
            }

            var data = line["data:".Length..].Trim();

            if (data.Length == 0)
            {
                continue;
            }

            if (data == "[DONE]")
            {
                yield break;
            }

            var chunk = JsonSerializer.Deserialize<ChatCompletionChunk>(data, SerializerOptions);
            var content = chunk?.Choices is { Count: > 0 } choices ? choices[0].Delta?.Content : null;

            if (!string.IsNullOrEmpty(content))
            {
                yield return content;
            }
        }
    }

    /// <summary>
    /// Сопоставляет роль сообщения с ролью OpenAI-совместимого API
    /// </summary>
    /// <param name="role">
    /// Роль отправителя
    /// </param>
    /// <returns>
    /// Строковое имя роли для API
    /// </returns>
    private static string MapRole(string role) =>
        role switch
        {
            // todo возможно потом переделать всю эту тему
            "System" => "system",
            "Assistant" => "assistant",
            "User" => "user",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };

    /// <summary>
    /// Извлекает текст ответа из тела синхронного ответа gen-api.
    /// Пробует известные поля (result / response / output), включая массивы строк
    /// и Anthropic-блоки вида { "text": "..." }
    /// </summary>
    /// <param name="body">
    /// Сырое тело ответа gen-api
    /// </param>
    /// <returns>
    /// Сгенерированный текст
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Формат ответа не распознан — текст извлечь не удалось
    /// </exception>
    private static string ExtractText(string body)
    {
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        foreach (var key in new[] { "result", "response", "output" })
        {
            if (root.TryGetProperty(key, out var element))
            {
                var text = ReadTextValue(element);

                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }
        }

        throw new HttpRequestException($"gen-api: не удалось извлечь текст из ответа: {body}");
    }

    /// <summary>
    /// Рекурсивно собирает текст из элемента JSON. Спускается по OpenAI-обёртке
    /// gen-api (choices → message → content) и понимает строки, массивы и
    /// Anthropic-блоки { "text": "..." }
    /// </summary>
    /// <param name="element">
    /// Элемент JSON
    /// </param>
    /// <returns>
    /// Собранный текст или пустая строка
    /// </returns>
    private static string ReadTextValue(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return element.GetString() ?? string.Empty;

            case JsonValueKind.Array:
                return string.Concat(element.EnumerateArray().Select(ReadTextValue));

            case JsonValueKind.Object when element.TryGetProperty("choices", out var choices):
                return ReadTextValue(choices);

            case JsonValueKind.Object when element.TryGetProperty("message", out var message):
                return ReadTextValue(message);

            case JsonValueKind.Object when element.TryGetProperty("content", out var content):
                return ReadTextValue(content);

            case JsonValueKind.Object when element.TryGetProperty("text", out var text):
                return text.GetString() ?? string.Empty;

            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// Строит тело запроса из модели запроса
    /// </summary>
    /// <param name="request">
    /// Запрос к AI-сервису
    /// </param>
    /// <returns>
    /// Тело запроса для gen-api
    /// </returns>
    private ChatCompletionRequest BuildPayload(AiRequest request) =>
        new ChatCompletionRequest
        {
            Model = string.IsNullOrWhiteSpace(request.Model) ? this.options.DefaultModel : request.Model,
            Temperature = request.Temperature,
            MaxTokens = request.MaxTokens,
            Messages = request.Messages.Select(
                    message => new ChatMessage
                    {
                        Role = MapRole(message.Role), Content = message.Content ?? string.Empty,
                    })
                .ToList(),
        };

    /// <summary>
    /// Тело запроса генерации
    /// </summary>
    private sealed class ChatCompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public IReadOnlyList<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        [JsonPropertyName("stream")]
        public bool? Stream { get; set; }

        [JsonPropertyName("is_sync")]
        public bool? IsSync { get; set; }
    }

    /// <summary>
    /// Сообщение в формате OpenAI-совместимого API
    /// </summary>
    private sealed class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }

    /// <summary>
    /// Фрагмент потокового ответа
    /// </summary>
    private sealed class ChatCompletionChunk
    {
        [JsonPropertyName("choices")]
        public IReadOnlyList<StreamChoice>? Choices { get; set; }
    }

    /// <summary>
    /// Вариант фрагмента потокового ответа
    /// </summary>
    private sealed class StreamChoice
    {
        [JsonPropertyName("delta")]
        public Delta? Delta { get; set; }
    }

    /// <summary>
    /// Дельта — приращение содержимого в потоковом ответе
    /// </summary>
    private sealed class Delta
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
