namespace Engine.Api.Endpoints;

using Engine.Api.EndpointsSetting;
using Engine.Api.Extensions;
using Engine.Api.Constants;
using Engine.Application.Features.Development;
using Engine.Application.Messaging;

using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Эндпоинт для разработки.
/// </summary>
internal sealed class DevelopmentEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{EngineRoutes.Parts.Dev}").WithTags("Development");

        group.MapPost("create-collections", SetVectorDbCollectionsHandler)
            .WithName("DevelopmentCreateSample")
            .WithDescription("Создает базовые коллекции в Qdrant.");
    }

    /// <summary>
    /// Создание коллекций в Qdrant
    /// </summary>
    /// <param name="sender">
    /// Медиатор
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Ответ
    /// </returns>
    private static async Task<Results<Ok<string>, ProblemHttpResult>> SetVectorDbCollectionsHandler(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateCollections.Command();

        // send
        var result = await sender.Send(command, cancellationToken);

        // return
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error.ToProblem();
    }
}
