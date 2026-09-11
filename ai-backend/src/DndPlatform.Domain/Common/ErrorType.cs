namespace DndPlatform.Domain.Common;

/// <summary>
/// Категория ошибки, определяющая её отображение на HTTP-статус.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Ошибка валидации входных данных (HTTP 400).
    /// </summary>
    Validation,

    /// <summary>
    /// Запрашиваемый ресурс не найден (HTTP 404).
    /// </summary>
    NotFound,

    /// <summary>
    /// Конфликт состояния ресурса (HTTP 409).
    /// </summary>
    Conflict,

    /// <summary>
    /// Запрос не авторизован (HTTP 401).
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Непредвиденная ошибка приложения (HTTP 500).
    /// </summary>
    Failure,

    /// <summary>
    /// Ошибка отсутствует.
    /// </summary>
    None,
}
