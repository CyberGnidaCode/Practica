namespace Engine.Application.Enums;

/// <summary>
/// Роли для сообщений AI
/// </summary>
public enum AiRole
{
    /// <summary>
    /// Системное сообщение (инструкции модели)
    /// </summary>
    System,

    /// <summary>
    /// Пользователь
    /// </summary>
    User,

    /// <summary>
    /// Ассистент (ответ модели)
    /// </summary>
    Assistant,
}