namespace Engine.Infrastructure.AiAdapters;

using Engine.Application.Enums;
using Engine.Application.Interfaces;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Фабрика клиентов AI-сервисов, разрешающая адаптер по провайдеру
/// через keyed-регистрацию в контейнере зависимостей
/// </summary>
public sealed class AiClientFactory : IAiClientFactory
{
    /// <summary>
    /// Провайдер служб
    /// </summary>
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AiClientFactory"/> class.
    /// Фабрика клиентов AI-сервисов
    /// </summary>
    /// <param name="serviceProvider">
    /// Провайдер служб для разрешения адаптера по ключу
    /// </param>
    public AiClientFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IAiClient Create(AiProvider provider) =>
        this.serviceProvider.GetRequiredKeyedService<IAiClient>(provider);
}
