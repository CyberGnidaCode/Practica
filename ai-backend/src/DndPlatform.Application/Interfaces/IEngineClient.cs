namespace DndPlatform.Application.Interfaces;

/// <summary>
/// Клиент движка
/// </summary>
public interface IEngineClient
{
    /// <summary>
    /// Запрашивает ответ нейросети на сообщение.
    /// </summary>
    /// <param name="message">
    /// Текст сообщения пользователя
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Текст ответа нейросети
    /// </returns>
    Task<string> CompleteAsync(string message, CancellationToken cancellationToken);
}
