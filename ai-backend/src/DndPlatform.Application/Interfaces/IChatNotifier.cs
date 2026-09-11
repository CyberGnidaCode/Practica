namespace DndPlatform.Application.Interfaces;

using DndPlatform.Application.Models.Messages;

/// <summary>
/// Рассылка сообщений участникам чата. Реализуется транспортным слоем (SignalR),
/// чтобы прикладной слой не зависел от деталей доставки.
/// </summary>
public interface IChatNotifier
{
    /// <summary>
    /// Рассылает новое сообщение всем участникам чата.
    /// </summary>
    /// <param name="chatId">
    /// Id чата
    /// </param>
    /// <param name="message">
    /// Сообщение
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    Task NotifyNewMessageAsync(Guid chatId, MessageDto message, CancellationToken cancellationToken);
}
