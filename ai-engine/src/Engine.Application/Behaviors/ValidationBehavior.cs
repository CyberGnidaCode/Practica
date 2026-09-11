namespace Engine.Application.Behaviors;

using System.Reflection;

using Engine.Application.Messaging;
using Engine.Domain.Common;

using FluentValidation;

/// <summary>
/// Поведение конвейера, выполняющее валидацию запроса через FluentValidation до его обработки.
/// Применяется только к запросам, возвращающим <see cref="Result"/> или <see cref="Result{TValue}"/>.
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса.</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    /// <summary>
    /// Фабрика создания неуспешного результата; используется для универсального создания Result.
    /// </summary>
    private static readonly MethodInfo FailureFactory = typeof(Result)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .First(method => method is { Name: nameof(Result.Failure), IsGenericMethod: true });

    /// <summary>
    /// Валидаторы запроса, зарегистрированные в контейнере.
    /// </summary>
    private readonly IEnumerable<IValidator<TRequest>> validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Валидаторы запроса, зарегистрированные в контейнере.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => this.validators = validators;

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> nextHandler,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nextHandler);

        if (!this.validators.Any())
        {
            return await nextHandler().ConfigureAwait(false);
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task
                          .WhenAll(
                              this.validators.Select(validator => validator.ValidateAsync(context, cancellationToken)))
                          .ConfigureAwait(false);

        var failures = results.SelectMany(result => result.Errors).Where(failure => failure is not null).ToList();

        if (failures.Count == 0)
        {
            return await nextHandler().ConfigureAwait(false);
        }

        var description = string.Join(" ", failures.Select(failure => failure.ErrorMessage));
        var error = ResultError.Validation("validation.failed", description);

        return CreateFailure(error);
    }

    /// <summary>
    /// Создаёт неуспешный результат <see cref="Result"/> или <see cref="Result{TValue}"/> с заданной ошибкой.
    /// </summary>
    /// <param name="error">Ошибка, которая будет установлена в результате.</param>
    /// <returns> Неуспешный результат.</returns>
    private static TResponse CreateFailure(ResultError error)
    {
        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            return (TResponse)Result.Failure(error);
        }

        var valueType = responseType.GetGenericArguments()[0];
        var failure = FailureFactory.MakeGenericMethod(valueType).Invoke(null, new object[] { error });

        return (TResponse)failure!;
    }
}
