namespace Engine.Application.Behaviors;

using Engine.Application.Diagnostics;
using Engine.Application.Messaging;
using Engine.Domain.Common;

/// <summary>
/// Поведение конвейера, обогащающее журнал текущего запроса прикладными сведениями: именем
/// запроса и — при неуспехе — кодом ошибки. Само ничего не пишет в лог; данные попадают в единую
/// итоговую строку HTTP-запроса (UseSerilogRequestLogging) через <see cref="IRequestDiagnostics"/>.
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса.</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
internal sealed class DiagnosticsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Журнал запроса.
    /// </summary>
    private readonly IRequestDiagnostics diagnostics;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticsBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="diagnostics">Служба обогащения журнала запроса.</param>
    public DiagnosticsBehavior(IRequestDiagnostics diagnostics) => this.diagnostics = diagnostics;

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> nextHandler,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nextHandler);

        var requestType = typeof(TRequest);

        // Команды/запросы вложены в фичу (CreateSample.Command), поэтому одного Name мало —
        // добавляем имя объемлющего типа, чтобы в логах было «CreateSample.Command».
        var requestName = requestType.DeclaringType is null
                              ? requestType.Name
                              : $"{requestType.DeclaringType.Name}.{requestType.Name}";

        this.diagnostics.SetRequestName(requestName);

        var response = await nextHandler().ConfigureAwait(false);

        if (response is Result { IsFailure: true } failed)
        {
            var error = failed.Error;
            this.diagnostics.SetError(error.Code, error.Description, error.Type.ToString());
        }

        return response;
    }
}
