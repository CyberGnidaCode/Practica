namespace Engine.Domain.Common;

/// <summary>
/// Описывает ошибку доменного или прикладного слоя.
/// </summary>
public sealed record ResultError
{
    /// <summary>
    /// Отсутствие ошибки. Используется для успешного результата.
    /// </summary>
    public static readonly ResultError None = new ResultError(string.Empty, string.Empty, ErrorType.None);

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultError"/> class.
    /// </summary>
    /// <param name="code">Машинно-читаемый код ошибки (например, <c>users.not_found</c>).</param>
    /// <param name="description">Человекочитаемое описание ошибки.</param>
    /// <param name="type">Категория ошибки.</param>
    public ResultError(string code, string description, ErrorType type)
    {
        this.Code = code;
        this.Description = description;
        this.Type = type;
    }

    /// <summary>
    /// Gets the machine-readable error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the error category.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Создаёт ошибку «ресурс не найден».
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="description">Описание ошибки.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.NotFound"/>.</returns>
    public static ResultError NotFound(string code, string description) =>
        new ResultError(code, description, ErrorType.NotFound);

    /// <summary>
    /// Создаёт ошибку валидации.
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="description">Описание ошибки.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Validation"/>.</returns>
    public static ResultError Validation(string code, string description) =>
        new ResultError(code, description, ErrorType.Validation);

    /// <summary>
    /// Создаёт ошибку конфликта.
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="description">Описание ошибки.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Conflict"/>.</returns>
    public static ResultError Conflict(string code, string description) =>
        new ResultError(code, description, ErrorType.Conflict);

    /// <summary>
    /// Создаёт непредвиденную ошибку приложения.
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="description">Описание ошибки.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Failure"/>.</returns>
    public static ResultError Failure(string code, string description) =>
        new ResultError(code, description, ErrorType.Failure);
}
