namespace Engine.Api.Endpoints;

using Engine.Api.EndpointsSetting;
using Engine.Api.Extensions;
using Engine.Api.Constants;
using Engine.Application.Features.AI;
using Engine.Application.Messaging;

using Microsoft.AspNetCore.Http.HttpResults;

using Engine.Application.Models;

/// <summary>
/// Эндпоинты для работы с чатами
/// </summary>
internal sealed class ChatEndpoints : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{EngineRoutes.Parts.Engine}").WithTags("Engine");

        group.MapPost("/complete", CompleteHandler)
            .WithName("Отправка сообщения к ИИ")
            .WithDescription("Отправляет сообщение в чат ИИ");
    }

    /// <summary>
    /// Обработчик отправки сообщения в чат ИИ
    /// </summary>
    /// <param name="message">
    /// Post-запрос отправки сообзения
    /// </param>
    /// <param name="sender">
    /// Медиатор
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Ответ ИИ
    /// </returns>
    private static async Task<Results<Ok<string>, ProblemHttpResult>> CompleteHandler(
        string message,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new Complete.Command
        {
            Messages = new[] { new Message { Id = Guid.NewGuid(), Role = "User", Content = message }, },
        };

        // send
        var result = await sender.Send(command, cancellationToken);

        // return
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error.ToProblem();
    }
}