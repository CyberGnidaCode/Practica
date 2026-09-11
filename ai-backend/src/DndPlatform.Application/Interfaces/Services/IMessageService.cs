namespace DndPlatform.Application.Interfaces.Services;

using DndPlatform.Application.Models.Messages;

/// <summary>
/// Интерфейс сервиса "Сообщения"
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Отправка сообщения
    /// </summary>
    /// <param name="message">
    /// Dto
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    Task SendMessageAsync(MessageDto message, CancellationToken cancellationToken);
}