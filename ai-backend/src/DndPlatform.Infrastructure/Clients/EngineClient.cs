namespace DndPlatform.Infrastructure.Clients;

using System.Net.Http.Json;

using DndPlatform.Application.Interfaces;

/// <summary>
/// Клиент движка
/// </summary>
internal sealed class EngineClient : IEngineClient
{
    /// <summary>
    /// Http-клиент (базовый адрес задаётся при регистрации).
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EngineClient"/>.
    /// </summary>
    /// <param name="httpClient">
    /// Http-клиент
    /// </param>
    public EngineClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Получить ответ от ИИ
    /// </summary>
    /// <param name="message">
    /// Сообщение todo передлать на массив
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Ответ
    /// </returns>
    public async Task<string> CompleteAsync(string message, CancellationToken cancellationToken)
    {
        // Эндпоинт принимает сообщение query-параметром (string message), тело пустое.
        var requestUri = new Uri($"complete?message={Uri.EscapeDataString(message)}", UriKind.Relative);

        using var response = await this.httpClient.PostAsync(requestUri, content: null, cancellationToken);

        response.EnsureSuccessStatusCode();

        // result
        var result = await response.Content.ReadFromJsonAsync<string>(cancellationToken);

        return result ?? string.Empty;
    }
}
