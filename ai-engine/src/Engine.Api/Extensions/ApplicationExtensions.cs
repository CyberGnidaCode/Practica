namespace Engine.Api.Extensions;

using Engine.Api.Logging;
using Engine.Application.Diagnostics;
using Engine.Application.Extensions;
using Engine.Infrastructure.Extensions;
using Engine.VectorDBContext.Extensions;

/// <summary>
/// Расширения для настройки приложения.
/// </summary>
internal static class ApplicationExtensions
{
    /// <summary>
    /// Настройка приложения.
    /// </summary>
    /// <param name="builder">
    /// Билдер приложения.
    /// </param>
    public static void TuneEngineApplication(this WebApplicationBuilder builder)
    {
        // variables
        var services = builder.Services;

        // tune services
        services.TuneServicesApplication(builder.Configuration);
    }

    /// <summary>
    /// Настройка сервисов.
    /// </summary>
    /// <param name="services">
    /// Коллекция сервисов.
    /// </param>
    /// <param name="configuration">
    /// Конфигурация приложения.
    /// </param>
    private static void TuneServicesApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Стандартизованные ответы об ошибках в формате ProblemDetails (RFC 9457).
        services.AddProblemDetails();

        // Обогащение единой итоговой строки запроса (UseSerilogRequestLogging) прикладными
        // сведениями из CQRS-пайплайна: имя запроса и код ошибки.
        services.AddHttpContextAccessor();
        services.AddSingleton<IRequestDiagnostics>(
            provider => new HttpContextRequestDiagnostics(provider.GetRequiredService<IHttpContextAccessor>()));

        // Прикладной слой: медиатор, обработчики, поведения конвейера и валидаторы фич.
        // Валидация входных данных выполняется в CQRS-пайплайне (ValidationBehavior), не в Web-слое.
        services.AddEngineApplication();

        // Инфраструктурный слой: адаптеры AI-сервисов и фабрика выбора провайдера.
        services.AddEngineInfrastructure(configuration);

        services.AddEngineVectorDbContext(configuration);
    }
}