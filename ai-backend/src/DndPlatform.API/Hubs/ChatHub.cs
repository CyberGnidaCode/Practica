namespace DndPlatform.Api.Hubs;

using System.Security.Claims;

using DndPlatform.Application.Interfaces;
using DndPlatform.Application.Interfaces.Services;
using DndPlatform.Application.Models.Messages;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Hub чата: приём сообщений от клиентов и управление группами по чатам.
/// </summary>
internal sealed class ChatHub : Hub<IChatClient>
{
    /// <summary>
    /// Сервис сообщений
    /// </summary>
    private readonly IMessageService messageService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ChatHub"/>.
    /// </summary>
    /// <param name="messageService">
    /// Сервис сообщений
    /// </param>
    public ChatHub(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    /// <summary>
    /// Присоединяет текущее подключение к группе чата, чтобы получать его сообщения.
    /// </summary>
    /// <param name="chatId">
    /// Id чата
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    public Task JoinChat(Guid chatId) =>
        this.Groups.AddToGroupAsync(this.Context.ConnectionId, chatId.ToString(), this.Context.ConnectionAborted);

    /// <summary>
    /// Отсоединяет текущее подключение от группы чата.
    /// </summary>
    /// <param name="chatId">
    /// Id чата
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    public Task LeaveChat(Guid chatId) =>
        this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, chatId.ToString(), this.Context.ConnectionAborted);

    /// <summary>
    /// Обрабатывает новое сообщение пользователя: делегирует его сервису, который
    /// сохраняет сообщение, запрашивает ответ нейросети и рассылает оба в группу чата.
    /// </summary>
    /// <param name="chatId">
    /// Id чата
    /// </param>
    /// <param name="message">
    /// Текст сообщения
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    public Task NewMessage(Guid chatId, string message)
    {
        var dto = new MessageDto { ChatId = chatId, Message = message, };

        return this.messageService.SendMessageAsync(dto, this.Context.ConnectionAborted);
    }

    /// <summary>
    /// Извлекает идентификатор пользователя из claim'ов подключения.
    /// </summary>
    /// <returns>
    /// Id пользователя или <see langword="null"/>, если он недоступен.
    /// </returns>
    private Guid? GetUserId()
    {
        var value = this.Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(value, out var userId) ? userId : null;
    }
}
