namespace Engine.Domain.Common;

/// <summary>
/// Представляет результат операции со значением.
/// </summary>
/// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
public sealed class Result<TValue> : Result
{
    /// <summary>
    /// Значение успешного результата. Доступно только если <see cref="Result.IsSuccess"/> равно true.
    /// </summary>
    private readonly TValue? value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">Значение результата.</param>
    /// <param name="isSuccess">Признак успешного результата.</param>
    /// <param name="error">Ошибка либо <see cref="ResultError.None"/> для успеха.</param>
    internal Result(TValue? value, bool isSuccess, ResultError error)
        : base(isSuccess, error) =>
        this.value = value;

    /// <summary>
    /// Gets the value of a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Результат является неуспешным.</exception>
    public TValue Value =>
        this.IsSuccess
            ? this.value!
            : throw new InvalidOperationException("Нельзя получить значение неуспешного результата.");
}
