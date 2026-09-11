namespace DndPlatform.Api.Extensions;

using DndPlatform.Api.Hubs;
using DndPlatform.Application.Extensions;
using DndPlatform.Application.Interfaces;
using DndPlatform.Infrastructure.Extensions;
using DndPlatform.Persistence;

/// <summary>
/// Расширения для настройки приложения.
/// </summary>
internal static class ApplicationExtensions
{
    /// <summary>
    /// Имя строки подключения к базе данных.
    /// </summary>
    private const string StorageConnectionName = "StorageContext";

    /// <summary>
    /// Ключ настройки базового адреса сервиса нейросети.
    /// </summary>
    private const string AiCompletionBaseAddressKey = "AiCompletion:BaseAddress";

    /// <summary>
    /// Настройка приложения.
    /// </summary>
    /// <param name="builder">
    /// Билдер приложения.
    /// </param>
    public static void TuneDndPlatformApplication(this WebApplicationBuilder builder)
    {
        // variables
        var services = builder.Services;

        services.AddSignalR();

        builder.Services.AddCors(
            o => o.AddPolicy(
                "client",
                p => p.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials())); // обязательно для SignalR

        services.AddScoped<IChatNotifier, ChatNotifier>();

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
    private static void TuneServicesApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Стандартизованные ответы об ошибках в формате ProblemDetails (RFC 9457).
        services.AddProblemDetails();

        // Прикладной слой.
        services.AddDndPlatformApplication();

        // Слой инфраструктуры
        var aiCompletionBaseAddress = configuration[AiCompletionBaseAddressKey]
                                      ?? throw new InvalidOperationException(
                                          $"Настройка '{AiCompletionBaseAddressKey}' не задана.");

        services.AddDndPlatformInfrastructure(aiCompletionBaseAddress);

        // Слой хранения (контекст бд).
        var connectionString = configuration.GetConnectionString(StorageConnectionName)
                               ?? throw new InvalidOperationException(
                                   $"Строка подключения '{StorageConnectionName}' не задана.");

        services.AddDndPlatformStorage(connectionString);
    }
}