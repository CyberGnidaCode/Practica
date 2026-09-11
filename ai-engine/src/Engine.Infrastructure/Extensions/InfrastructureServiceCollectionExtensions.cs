namespace Engine.Infrastructure.Extensions;

using System.Net.Http.Headers;

using Engine.Application.Enums;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Infrastructure.AiAdapters;
using Engine.Infrastructure.Options;
using Engine.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Регистрация служб инфраструктурного слоя: адаптеров AI-сервисов и их фабрики
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет службы инфраструктурного слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов с зарегистрированными службами инфраструктурного слоя.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="services"/> равен <see langword="null"/>.</exception>
    public static IServiceCollection AddEngineInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // фабрика выбора адаптера по провайдеру
        services.AddSingleton<IAiClientFactory, AiClientFactory>();

        // адаптеры AI-сервисов: каждый регистрируется под ключом-провайдером.
        // Новый провайдер = новый адаптер + один блок регистрации ниже.
        services.AddKeyedTransient<IAiClient, TestAiAdapter>(AiProvider.Test);

        AddOpenRouter(services, configuration);
        services.AddTransient<IExtractorService, ExtractorService>();

        return services;
    }

    /// <summary>
    /// Регистрирует адаптер OpenRouter как типизированный HTTP-клиент с настройками,
    /// авторизацией и стандартным конвейером устойчивости (повторы, таймауты, circuit breaker)
    /// </summary>
    /// <param name="services">
    /// Коллекция сервисов
    /// </param>
    /// <param name="configuration">
    /// Конфигурация приложения
    /// </param>
    private static void AddOpenRouter(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenRouterOptions>(configuration.GetSection(OpenRouterOptions.SectionName));

        services.AddHttpClient<OpenRouterAdapter>(
                (provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<OpenRouterOptions>>().Value;

                    client.BaseAddress = new Uri("https://api.gen-api.ru/api/v1/");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", options.ApiKey);

                    if (!string.IsNullOrWhiteSpace(options.Referer))
                    {
                        client.DefaultRequestHeaders.Add("HTTP-Referer", options.Referer);
                    }

                    if (!string.IsNullOrWhiteSpace(options.Title))
                    {
                        client.DefaultRequestHeaders.Add("X-Title", options.Title);
                    }
                })
            .AddStandardResilienceHandler(
                resilience =>
                {
                    // LLM-запросы (особенно к локальной модели) отвечают дольше,
                    // чем стандартные 10 c на попытку. Даём генерации завершиться.
                    resilience.AttemptTimeout.Timeout = TimeSpan.FromMinutes(2);
                    resilience.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(2);

                    // Валидатор Polly требует SamplingDuration >= 2 * AttemptTimeout.
                    resilience.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(4);

                    // Повтор пересылает весь промпт и запускает генерацию заново —
                    // для дорогих завершений ограничиваемся одной повторной попыткой.
                    resilience.Retry.MaxRetryAttempts = 1;
                });

        // Ключевая регистрация: фабрика разрешает адаптер по провайдеру,
        // а сам экземпляр берётся из типизированного HTTP-клиента выше.
        services.AddKeyedTransient<IAiClient, OpenRouterAdapter>(
            AiProvider.OpenRouter,
            (provider, _) => provider.GetRequiredService<OpenRouterAdapter>());
    }
}
