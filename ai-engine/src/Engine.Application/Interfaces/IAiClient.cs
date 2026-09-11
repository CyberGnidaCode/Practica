namespace Engine.Application.Interfaces;

using Engine.Application.Models;

/// <summary>
/// Интерфейс клиента AI-сервиса
/// </summary>
public interface IAiClient
{
    /// <summary>
    /// Получает ответ от AI-сервиса на основе переданного промпта
    /// </summary>
    /// <param name="request">
    /// Запрос к AI-сервису
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Ответ от AI-сервиса
    /// </returns>
    Task<string> CompleteAsync(AiRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Потоково получает ответ от AI-сервиса: возвращает фрагменты текста по мере их генерации
    /// </summary>
    /// <param name="request">
    /// Запрос к AI-сервису
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Асинхронная последовательность фрагментов ответа
    /// </returns>
    IAsyncEnumerable<string> StreamAsync(AiRequest request, CancellationToken cancellationToken);
}