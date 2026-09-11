namespace Engine.Api.EndpointsSetting;

/// <summary>
/// Defines a contract for mapping API endpoints.
/// </summary>
internal interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// </summary>
    /// <param name="app">
    ///  The route builder for grouping and mapping endpoints.
    /// </param>
    void MapEndpoint(IEndpointRouteBuilder app);
}