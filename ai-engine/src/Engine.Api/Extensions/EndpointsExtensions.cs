namespace Engine.Api.Extensions;

using System.Reflection;

using Engine.Api.EndpointsSetting;

using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Расширени для регистрации API эндпоинтов.
/// </summary>
internal static class EndpointsExtensions
{
    /// <summary>
    /// Добавляет API эндпоинты из указанной сборки в коллекцию сервисов.
    /// </summary>
    /// <param name="services">
    /// Коллекция сервисов.
    /// </param>
    /// <param name="assembly">
    /// Сборка.
    /// </param>
    /// <returns>
    /// Коллекция сервисов с зарегистрированными API эндпоинтами.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если параметр <paramref name="assembly"/> равен <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        // check
        ArgumentNullException.ThrowIfNull(assembly);

        // register endpoints
        var serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));

        // add to services
        services.TryAddEnumerable(serviceDescriptors);

        // return
        return services;
    }

    /// <summary>
    /// Регистрирует API эндпоинты в конвейере обработки HTTP-запросов.
    /// </summary>
    /// <param name="app">Приложение.</param>
    /// <param name="routeGroupBuilder">Строитель группы маршрутов.</param>
    /// <returns>Приложение с зарегистрированными API эндпоинтами.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если параметр <paramref name="app"/> равен <see langword="null"/>.</exception>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        // check
        ArgumentNullException.ThrowIfNull(app);

        // get builder
        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        // map endpoints
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        // return
        return app;
    }
}