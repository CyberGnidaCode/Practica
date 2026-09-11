namespace Engine.VectorDBContext.Services;

using Engine.VectorDBContext.Interfaces;

using Qdrant.Client.Grpc;

/// <summary>
/// Временная заглушка <see cref="IEmbeddingClient"/>: возвращает детерминированный
/// фиктивный вектор, чтобы приложение запускалось до появления реального клиента.
/// </summary>
public sealed class EmbeddingClient : IEmbeddingClient, IDisposable
{
    /// <summary>
    /// связь с питоном.
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddingClient"/> class.
    /// конструктор для векторов.
    /// </summary>
    /// <param name="baseAddress">из строки подключения</param>
    public EmbeddingClient(Uri baseAddress)
    {
        this.httpClient = new HttpClient { BaseAddress = baseAddress };
    }

    /// <summary>
    /// Получает векторное представление текста.
    /// </summary>
    /// <param name="text">Текст для векторизации.</param>
    /// <returns>Векторное представление текста.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если текст равен null.</exception>
    public async Task<List<PointStruct>> EmbedAsync(string[] text)
    {
        // check
        ArgumentNullException.ThrowIfNull(text);

        var request = new { texts = text };
        var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
        using var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        var response = await this.httpClient.PostAsync(new Uri("/embed", UriKind.Relative), content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var raw = System.Text.Json.JsonSerializer.Deserialize<List<EmbedResponse>>(jsonResponse);

        var embedResult = new List<PointStruct>();
        if (raw != null)
        {
            foreach (var part in raw)
            {
                var point = new PointStruct
                {
                    Id = new PointId { Uuid = part!.id },
                    Vectors = part.vector,
                    Payload = { ["text"] = part.payload["text"].ToString() ?? string.Empty },
                };
                embedResult.Add(point);
            }
        }

        return embedResult;
    }

    private sealed record EmbedResponse(
        string id,
        float[] vector,
        Dictionary<string, object> payload);

    /// <summary>
    /// высвобождение клиента
    /// </summary>
    public void Dispose()
    {
        this.httpClient.Dispose();
    }
}
