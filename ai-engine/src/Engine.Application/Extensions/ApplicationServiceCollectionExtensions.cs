namespace Engine.Application.Extensions;

using Engine.Application.Behaviors;
using Engine.Application.Messaging;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Регистрация служб прикладного слоя: медиатора, обработчиков, поведений конвейера и валидаторов.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет службы прикладного слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с зарегистрированными службами прикладного слоя.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    public static IServiceCollection AddEngineApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assembly = typeof(ApplicationServiceCollectionExtensions).Assembly;

        // медиатор
        services.AddScoped<ISender, Sender>();

        // обработчики запросов: каждый закрытый IRequestHandler<,>
        var handlerInterface = typeof(IRequestHandler<,>);

        foreach (var type in assembly.GetTypes().Where(type => type is { IsAbstract: false, IsInterface: false }))
        {
            var contracts = type.GetInterfaces()
                .Where(contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == handlerInterface);

            foreach (var contract in contracts)
            {
                services.AddTransient(contract, type);
            }
        }

        // поведения конвейера (открытый дженерик); порядок регистрации = порядок выполнения.
        // DiagnosticsBehavior регистрируется первым → он внешний и видит итоговый результат, включая валидацию.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DiagnosticsBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // валидаторы FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
