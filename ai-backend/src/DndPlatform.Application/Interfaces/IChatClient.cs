namespace DndPlatform.Application.Interfaces;

using DndPlatform.Application.Models.Messages;

/// <summary>
/// Клиент
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// Принять сообщение
    /// </summary>
    /// <param name="message">
    /// Сообщение
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    Task ReceiveMessage(MessageDto message);
}