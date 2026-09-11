namespace Engine.VectorDBContext.Extensions;

using Engine.Application.Interfaces.VectorStorage;
using Engine.VectorDBContext.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Qdrant.Client;

/// <summary>
/// Регистрация служб
/// </summary>
public static class VectorDbContextServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет службы прикладного слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация</param>
    /// <returns>Коллекция сервисов с зарегистрированными службами прикладного слоя.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    public static IServiceCollection AddEngineVectorDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        var connectionString = configuration.GetConnectionString("Qdrant");
        var embeddingConnectionString = configuration.GetConnectionString("EmbedClient");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Не задана строка подключения 'Qdrant'.");
        }

        if (string.IsNullOrWhiteSpace(embeddingConnectionString))
        {
            throw new InvalidOperationException("Не задана строка подключения 'Embedding'.");
        }

        // qdrant client
        services.AddSingleton(_ => new QdrantClient(new Uri(connectionString)));

        // embedding client
        services.AddSingleton(_ => new EmbeddingClient(new Uri(embeddingConnectionString)));

        // services
        services.AddScoped<IVectorStorageService, VectorStorageService>();

        // return
        return services;
    }
}