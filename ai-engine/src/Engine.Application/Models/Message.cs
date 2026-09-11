namespace Engine.Application.Models;

/// <summary>
/// Модель сообщения AI
/// </summary>
public class Message
{
    /// <summary>
    /// Id сообщения
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Роль отправителя сообщения
    /// </summary>
    public required string Role { get; set; }

    /// <summary>
    /// Контент
    /// </summary>
    public string Content { get; set; } = string.Empty;
}