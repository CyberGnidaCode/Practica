namespace Engine.Application.Interfaces.Services;

/// <summary>
/// Сервис работы с внутренней ии.
/// </summary>
public interface IExtractorService
{
    /// <summary>
    /// Получение саммари из ответа ии.
    /// </summary>
    /// <param name="text">Исходный текст</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Саммари</returns>
    public Task<string> SummarizeAsync(string text, CancellationToken cancellationToken);

    /// <summary>
    /// Получение embedding из текста.
    /// </summary>
    /// <param name="text">Исходный текст</param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>Саммари</returns>
    Task<string> EmbeddingAsync(string text, CancellationToken cancellationToken);
}