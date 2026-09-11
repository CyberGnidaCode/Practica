namespace Engine.Domain.Common;

/// <summary>
/// Представляет результат операции: успех либо ошибку.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Признак успешного результата.</param>
    /// <param name="error">Ошибка либо <see cref="ResultError.None"/> для успеха.</param>
    /// <exception cref="ArgumentException">Состояние успеха не согласовано с наличием ошибки.</exception>
    protected Result(bool isSuccess, ResultError error)
    {
        if (isSuccess && error != ResultError.None)
        {
            throw new ArgumentException("Успешный результат не может содержать ошибку.", nameof(error));
        }

        if (!isSuccess && error == ResultError.None)
        {
            throw new ArgumentException("Неуспешный результат обязан содержать ошибку.", nameof(error));
        }

        this.IsSuccess = isSuccess;
        this.Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !this.IsSuccess;

    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    public ResultError Error { get; }

    /// <summary>
    /// Создаёт успешный результат без значения.
    /// </summary>
    /// <returns>Успешный <see cref="Result"/>.</returns>
    public static Result Success() => new Result(true, ResultError.None);

    /// <summary>
    /// Создаёт неуспешный результат.
    /// </summary>
    /// <param name="error">Ошибка.</param>
    /// <returns>Неуспешный <see cref="Result"/>.</returns>
    public static Result Failure(ResultError error) => new Result(false, error);

    /// <summary>
    /// Создаёт успешный результат со значением.
    /// </summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    /// <param name="value">Значение результата.</param>
    /// <returns>Успешный <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, ResultError.None);

    /// <summary>
    /// Создаёт неуспешный результат со значением.
    /// </summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    /// <param name="error">Ошибка.</param>
    /// <returns>Неуспешный <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Failure<TValue>(ResultError error) => new Result<TValue>(default, false, error);
}
