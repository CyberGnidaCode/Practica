namespace DndPlatform.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Регистрация служб слоя хранения.
/// </summary>
public static class StorageServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет контекст бд в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    /// <returns>Коллекция сервисов с зарегистрированным контекстом бд.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Параметр <paramref name="connectionString"/> пустой или равен <see langword="null"/>.</exception>
    public static IServiceCollection AddDndPlatformStorage(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<DndPlatform.Persistence.StorageContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
