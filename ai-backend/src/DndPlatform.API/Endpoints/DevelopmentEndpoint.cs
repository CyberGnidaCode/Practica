namespace DndPlatform.Api.Endpoints;

using DndPlatform.Api.Constants;
using DndPlatform.Api.EndpointsSetting;
using DndPlatform.Domain.Constants;
using DndPlatform.Persistence;
using DndPlatform.Persistence.Models;

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

        group.MapPost("/test-chat", CreateTestChatAsync)
            .WithName("CreateTestChat")
            .WithSummary("Creates an isolated chat for SignalR testing.");

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

    private static async Task<IResult> CreateTestChatAsync(StorageContext context, CancellationToken cancellationToken)
    {
        var requiredSenderTypes = new[]
        {
            new SenderType { Name = SenderTypeConstants.System, Code = SenderTypeConstants.System },
            new SenderType { Name = SenderTypeConstants.User, Code = SenderTypeConstants.User },
            new SenderType { Name = SenderTypeConstants.Assistant, Code = SenderTypeConstants.Assistant },
        };

        var existingCodes = await context.SenderTypes
            .Select(senderType => senderType.Code)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        context.SenderTypes.AddRange(requiredSenderTypes.Where(senderType => !existingCodes.Contains(senderType.Code)));

        var template = new Template
        {
            Name = "Test template",
            Desription = "Temporary template for SignalR chat testing.",
            Setting = "Development",
            RedFlag = string.Empty,
            IsPublic = false,
        };
        var master = new Master { Name = "Test master", Description = "Temporary test master." };
        var game = new Game { Template = template, Master = master };
        var chat = new Chat { Game = game };

        context.Chats.Add(chat);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return TypedResults.Ok(new { chatId = chat.Id });
    }
}
