namespace DndPlatform.Api.Models.Messages.Requests;

/// <summary>
/// Запрос на создание/отправку сообщения
/// </summary>
internal sealed class MessagePostRequest
{
    /// <summary>
    /// Чат
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Тип отправителя
    /// </summary>
    public Guid SenderTypeId { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public required string Text { get; set; }
}