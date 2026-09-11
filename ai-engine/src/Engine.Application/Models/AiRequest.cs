namespace Engine.Application.Models;

/// <summary>
/// Модель запроса к AI
/// </summary>
public class AiRequest
{
    /// <summary>
    /// Список сообщений
    /// </summary>
    public IReadOnlyList<Message> Messages { get; set; }

    /// <summary>
    /// Температура генерации текста (от 0 до 1)
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Максимальное количество токенов в ответе
    /// </summary>
    public int? MaxTokens { get; set; } = 10000; // default

    /// <summary>
    /// Идентификатор модели внутри провайдера (например, "openai/gpt-4o-mini").
    /// Если не задан — используется модель по умолчанию из настроек адаптера
    /// </summary>
    public string? Model { get; set; }
}