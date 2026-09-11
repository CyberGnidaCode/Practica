namespace Engine.Infrastructure.Options;

/// <summary>
/// Настройки адаптера OpenRouter (openrouter.ai)
/// </summary>
public sealed class OpenRouterOptions
{
    /// <summary>
    /// Имя секции конфигурации
    /// </summary>
    public const string SectionName = "Ai:OpenRouter";

    /// <summary>
    /// Ключ API OpenRouter (заголовок Authorization: Bearer)
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Модель по умолчанию, если она не указана в запросе (например, "openai/gpt-4o-mini")
    /// </summary>
    public string DefaultModel { get; set; } = "openai/gpt-4o-mini";

    /// <summary>
    /// Значение заголовка HTTP-Referer — источник запроса для рейтинга приложений OpenRouter (необязательно)
    /// </summary>
    public string? Referer { get; set; }

    /// <summary>
    /// Значение заголовка X-Title — имя приложения для рейтинга OpenRouter (необязательно)
    /// </summary>
    public string? Title { get; set; }
}
