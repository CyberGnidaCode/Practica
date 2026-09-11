namespace DndPlatform.Api.Extensions;

using DndPlatform.Domain.Common;

using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Преобразует доменные ошибки в стандартные HTTP-ответы ProblemDetails (RFC 9457).
/// </summary>
internal static class ResultExtensions
{
    /// <summary>
    /// Преобразует <see cref="ResultError"/> в ответ <see cref="ProblemHttpResult"/>.
    /// </summary>
    /// <param name="error">Доменная ошибка.</param>
    /// <returns>Ответ ProblemDetails с соответствующим HTTP-статусом.</returns>
    /// <exception cref="ArgumentNullException">Параметр <paramref name="error"/> равен <see langword="null"/>.</exception>
    public static ProblemHttpResult ToProblem(this ResultError error)
    {
        // check
        ArgumentNullException.ThrowIfNull(error);

        // map error type to HTTP status code
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        // expose machine-readable error code as a ProblemDetails extension
        var extensions = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["errorCode"] = error.Code,
        };

        // build standardized problem response
        return TypedResults.Problem(detail: error.Description, statusCode: statusCode, extensions: extensions);
    }
}
