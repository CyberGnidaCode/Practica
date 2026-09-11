namespace DndPlatform.Application.Models.Messages;

/// <summary>
/// Dto отправки сообщения
/// </summary>
public sealed class MessageDto
{
    /// <summary>
    /// Id чата
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public required string Message { get; set; }
}