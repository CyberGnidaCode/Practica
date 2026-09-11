namespace Engine.Api.Models.Chats.Requests;

/// <summary>
/// Post-Зарпос на отправку сообщения
/// </summary>
internal sealed class MessagePostRequest
{
    /// <summary>
    /// Id сообщения
    /// </summary>
    public Guid ChatId { get; init; }

    /// <summary>
    /// Сообщение пользователя
    /// </summary>
    public string Message { get; init; }
}