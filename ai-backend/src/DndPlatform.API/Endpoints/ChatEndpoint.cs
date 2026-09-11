namespace DndPlatform.Api.Endpoints;

using DndPlatform.Api.Constants;
using DndPlatform.Api.EndpointsSetting;
using DndPlatform.Api.Hubs;

/// <summary>
/// Эндпоинт для чата
/// </summary>
internal sealed class ChatEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{DndPlatformRoutes.Parts.Chat}").WithTags("Chat");

        group.MapHub<ChatHub>("/hub").WithDescription("Hub чата").WithSummary("Работа с комнатой чата");
    }
}