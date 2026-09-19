namespace DndPlatform.Infrastructure.Servises.Entities;

using DndPlatform.Application.Interfaces;
using DndPlatform.Application.Interfaces.Services;
using DndPlatform.Application.Models.Messages;
using DndPlatform.Domain.Constants;
using DndPlatform.Persistence;
using DndPlatform.Persistence.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Сервис [Сообщения]
/// </summary>
public class MessageService : ServiceBase<Message>, IMessageService
{
    /// <summary>
    /// Контекст бд
    /// </summary>
    private readonly StorageContext context;

    /// <summary>
    /// Клиент нейросети
    /// </summary>
    private readonly IEngineClient engineClient;

    /// <summary>
    /// Рассылка сообщений в чат
    /// </summary>
    private readonly IChatNotifier chatNotifier;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageService"/>
    /// </summary>
    /// <param name="context">
    /// Контекст бд
    /// </param>
    /// <param name="messageRepository">
    /// Репозиторий сообщений
    /// </param>
    /// <param name="engineClient">
    /// Клиент дижка
    /// </param>
    /// <param name="chatNotifier">
    /// Рассылка сообщений в чат
    /// </param>
    public MessageService(
        StorageContext context,
        IRepository<Message> messageRepository,
        IEngineClient engineClient,
        IChatNotifier chatNotifier)
        : base(context, messageRepository)
    {
        this.context = context;
        this.engineClient = engineClient;
        this.chatNotifier = chatNotifier;
    }

    /// <summary>
    /// Отправка сообщения: сохраняет сообщение пользователя, запрашивает ответ
    /// нейросети, сохраняет его и рассылает оба сообщения участникам чата.
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
    public async Task SendMessageAsync(MessageDto message, CancellationToken cancellationToken)
    {
        // сообщение пользователя
        var userMessage = await this.SaveMessageAsync(
                              message.ChatId,
                              SenderTypeConstants.User,
                              message.Message,
                              cancellationToken);

        await this.chatNotifier.NotifyNewMessageAsync(message.ChatId, ToDto(userMessage), cancellationToken);

        // response
        var answer = await this.engineClient.CompleteAsync(message.Message, cancellationToken);

        var aiMessage = await this.SaveMessageAsync(
                            message.ChatId,
                            SenderTypeConstants.Assistant,
                            answer,
                            cancellationToken);

        await this.chatNotifier.NotifyNewMessageAsync(message.ChatId, ToDto(aiMessage), cancellationToken);
    }

    /// <summary>
    /// todo убрать
    /// Преобразует сущность сообщения в Dto для рассылки.
    /// </summary>
    /// <param name="message">
    /// Сообщение
    /// </param>
    /// <returns>
    /// Dto сообщения
    /// </returns>
    private static MessageDto ToDto(Message message) =>
        new MessageDto { ChatId = message.ChatId, Message = message.Text };

    /// <summary>
    /// Сохраняет сообщение в чат от указанного типа отправителя.
    /// </summary>
    /// <param name="chatId">
    /// Id чата
    /// </param>
    /// <param name="senderTypeCode">
    /// Код типа отправителя
    /// </param>
    /// <param name="text">
    /// Текст сообщения
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Сохранённое сообщение
    /// </returns>
    private async Task<Message> SaveMessageAsync(
        Guid chatId,
        string senderTypeCode,
        string text,
        CancellationToken cancellationToken)
    {
        // get
        var senderTypeId = await this.context.SenderTypes.AsNoTracking()
                               .Where(senderType => senderType.Code == senderTypeCode)
                               .Select(senderType => senderType.Id)
                               .FirstOrDefaultAsync(cancellationToken);

        if (senderTypeId == Guid.Empty)
        {
            throw new InvalidOperationException($"Тип отправителя '{senderTypeCode}' не найден.");
        }

        var message = new Message
        {
            ChatId = chatId, SenderTypeId = senderTypeId, Text = text,
        };

        await this.AddAsync(message, cancellationToken);

        return message;
    }
}
