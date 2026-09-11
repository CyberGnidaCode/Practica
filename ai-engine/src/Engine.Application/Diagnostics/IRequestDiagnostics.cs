namespace Engine.Application.Diagnostics;

/// <summary>
/// Абстракция для обогащения журнала текущего запроса прикладными сведениями
/// </summary>
public interface IRequestDiagnostics
{
    /// <summary>
    /// Устанавливает имя обрабатываемого запроса.
    /// </summary>
    /// <param name="requestName">
    /// Имя запроса, например «CreateSample.Command».
    /// </param>
    void SetRequestName(string requestName);

    /// <summary>
    /// Устанавливает сведения об ошибке, которой завершился запрос.
    /// </summary>
    /// <param name="code">Машинно-читаемый код ошибки (например, <c>validation.failed</c>).</param>
    /// <param name="description">Человекочитаемое описание ошибки.</param>
    /// <param name="type">Категория ошибки (например, <c>Validation</c>).</param>
    void SetError(string code, string description, string type);
}
