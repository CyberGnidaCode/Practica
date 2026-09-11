namespace Engine.Application.Enums;

/// <summary>
/// Перечисление провайдеров AI-сервисов
/// </summary>
public enum AiProvider
{
    /// <summary>
    /// test
    /// </summary>
    Test,

    /// <summary>
    /// OpenAI
    /// </summary>
    OpenAi,

    /// <summary>
    /// Anthropic
    /// </summary>
    Anthropic,

    /// <summary>
    /// OpenRouter (openrouter.ai) — агрегатор моделей с OpenAI-совместимым API
    /// </summary>
    OpenRouter,

    /// <summary>
    /// Локальный роутер
    /// </summary>
    Local,
}