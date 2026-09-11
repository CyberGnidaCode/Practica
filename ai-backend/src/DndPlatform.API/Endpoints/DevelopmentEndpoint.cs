namespace DndPlatform.Api.Endpoints;

using DndPlatform.Api.Constants;
using DndPlatform.Api.EndpointsSetting;
using DndPlatform.Persistence;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Эндпоинт для разработки.
/// </summary>
internal sealed class DevelopmentEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{DndPlatformRoutes.Parts.Dev}").WithTags("Development");

        group.MapPost("/migrations/apply", ApplyMigrationsAsync)
            .WithName("ApplyMigrations")
            .WithSummary("Применяет ожидающие миграции базы данных.");
    }

    /// <summary>
    /// Применяет ожидающие миграции базы данных.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список применённых миграций.</returns>
    private static async Task<IResult> ApplyMigrationsAsync(StorageContext context, CancellationToken cancellationToken)
    {
        var pendingMigrations = await context.Database
                                    .GetPendingMigrationsAsync(cancellationToken)
                                    .ConfigureAwait(false);

        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        return TypedResults.Ok(pendingMigrations);
    }
}
