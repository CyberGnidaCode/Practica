namespace Engine.VectorDBContext.Services;

using Engine.Application.Interfaces.VectorStorage;
using Engine.VectorDBContext.Constants;

using Microsoft.SemanticKernel.Text;

using Qdrant.Client;
using Qdrant.Client.Grpc;

#pragma warning disable SKEXP0050

/// <summary>
/// Сервис для работы с векторной бд
/// </summary>
public sealed class VectorStorageService : IVectorStorageService
{
    /// <summary>
    /// Клиент
    /// </summary>
    private readonly QdrantClient qdrantClient;

    /// <summary>
    /// Клиент сервиса эмбеддингов
    /// </summary>
    private readonly EmbeddingClient embeddingClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="VectorStorageService"/> class.
    /// Сервис для работы с векторной бд
    /// </summary>
    /// <param name="qdrantClient">
    /// Клиент
    /// </param>
    /// <param name="embeddingClient">
    /// Клиент сервиса эмбеддингов
    /// </param>
    public VectorStorageService(QdrantClient qdrantClient, EmbeddingClient embeddingClient)
    {
        this.qdrantClient = qdrantClient;
        this.embeddingClient = embeddingClient;
    }

    /// <summary>
    /// добавление записи в Qdrant
    /// </summary>
    /// <param name="text">текст на вход</param>
    /// <param name="gameId">айди партии</param>
    /// <returns>Task</returns>
    public async Task AddPointAsync(string text, Guid gameId)
    {
        // убрал чанки
        // var chunks = Split(text).ToArray();
        var vectors = await this.embeddingClient.EmbedAsync(new[] { text });
        ArgumentNullException.ThrowIfNull(vectors);
        await this.qdrantClient.UpsertAsync("main", vectors);
    }

    /// todo доработать поиск по guidId
    /// <inheritdoc/>
    public async Task<List<string>> SearchAsync(string text, Guid gameId)
    {
        var queryText = new[] { text };
        var queryVector = await this.embeddingClient.EmbedAsync(queryText);

        var rawVector = queryVector?[0].Vectors.Vector.Dense.Data.ToArray();

        if (rawVector == null)
        {
            return new List<string>();
        }

        var results = await this.qdrantClient.QueryAsync(
                          collectionName: "main",
                          query: rawVector,
                          limit: 3,
                          payloadSelector: true);

        if (results.Count == 0)
        {
            return new List<string>();
        }

        return results.Select(r => r.Payload.TryGetValue("text", out var value) ? value.StringValue : string.Empty)
            .Where(t => !string.IsNullOrEmpty(t))
            .ToList();
    }

    /// <inheritdoc/>
    public async Task SetColletionsAsync()
    {
        // create main collection
        await this.qdrantClient.CreateCollectionAsync(
            collectionName: VectorDBCollections.Main,
            vectorsConfig: new VectorParams { Size = 1024, Distance = Distance.Cosine });

        // Индекс по gameId с флагом тенанта: физически группирует точки одной
        // игры на диске, ускоряя поиск с фильтром по gameId.
        await this.qdrantClient.CreatePayloadIndexAsync(
            collectionName: VectorDBCollections.Main,
            fieldName: "gameId",
            schemaType: PayloadSchemaType.Keyword,
            indexParams: new PayloadIndexParams { KeywordIndexParams = new KeywordIndexParams { IsTenant = true }, });
    }

    /// <summary>
    /// разбиение текста на чанки.
    /// </summary>
    /// <param name="text">изначальный текст</param>
    /// <returns>список чанков</returns>
    /// <exception cref="ArgumentNullException">нет текста</exception>
    private static List<string> Split(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var lines = TextChunker.SplitPlainTextLines(text, 100);
        var chunks = TextChunker.SplitPlainTextParagraphs(lines, 300, 50);

        return chunks;
    }
}