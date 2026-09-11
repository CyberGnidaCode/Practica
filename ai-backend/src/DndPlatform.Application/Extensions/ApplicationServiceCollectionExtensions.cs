namespace DndPlatform.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Регистрация служб прикладного слоя.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет службы прикладного слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с зарегистрированными службами прикладного слоя.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    public static IServiceCollection AddDndPlatformApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
