namespace DndPlatform.Infrastructure.Extensions;

using DndPlatform.Application.Interfaces;
using DndPlatform.Application.Interfaces.Services;
using DndPlatform.Infrastructure.Clients;
using DndPlatform.Infrastructure.Repositories;
using DndPlatform.Infrastructure.Servises;
using DndPlatform.Infrastructure.Servises.Entities;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Регистрация служб слоя инфраструктуры.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет службы слоя инфраструктуры в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="aiCompletionBaseAddress">Базовый адрес внешнего сервиса нейросети.</param>
    /// <returns>Коллекция сервисов с зарегистрированными службами слоя инфраструктуры.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    public static IServiceCollection AddDndPlatformInfrastructure(
        this IServiceCollection services,
        string aiCompletionBaseAddress)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(aiCompletionBaseAddress);

        // repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // services
        services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
        services.AddScoped<IMessageService, MessageService>();

        // clients
        services.AddHttpClient<IEngineClient, EngineClient>(
            client => client.BaseAddress = new Uri(aiCompletionBaseAddress));

        // return
        return services;
    }
}
