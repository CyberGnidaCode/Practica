namespace Engine.VectorDBContext.Interfaces;

using Qdrant.Client.Grpc;

/// <summary>
/// интерфейс клиента для получения векторного представления текста.
/// </summary>
public interface IEmbeddingClient
{
    /// <summary>
    /// Получает векторное представление текста.
    /// </summary>
    /// <param name="text">Текст для векторизации.</param>
    /// <returns>Векторное представление текста.</returns>
    Task<List<PointStruct>> EmbedAsync(string[] text);
}
