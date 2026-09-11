namespace DndPlatform.Api.Hubs;

using DndPlatform.Application.Interfaces;
using DndPlatform.Application.Models.Messages;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Реализация <see cref="IChatNotifier"/> поверх SignalR: рассылает сообщения
/// участникам группы чата через <see cref="ChatHub"/>.
/// </summary>
internal sealed class ChatNotifier : IChatNotifier
{
    /// <summary>
    /// Контекст хаба чата.
    /// </summary>
    private readonly IHubContext<ChatHub, IChatClient> hubContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ChatNotifier"/>.
    /// </summary>
    /// <param name="hubContext">
    /// Контекст хаба чата
    /// </param>
    public ChatNotifier(IHubContext<ChatHub, IChatClient> hubContext)
    {
        this.hubContext = hubContext;
    }

    /// <inheritdoc />
    public Task NotifyNewMessageAsync(Guid chatId, MessageDto message, CancellationToken cancellationToken) =>
        this.hubContext.Clients.Group(chatId.ToString()).ReceiveMessage(message);
}
