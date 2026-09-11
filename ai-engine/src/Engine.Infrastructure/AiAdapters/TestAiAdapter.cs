namespace Engine.Infrastructure.AiAdapters;

using System.Runtime.CompilerServices;

using Engine.Application.Interfaces;
using Engine.Application.Models;

/// <summary>
/// Тестовый адаптер AI-сервиса
/// </summary>
public sealed class TestAiAdapter : IAiClient
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
    public Task<string> CompleteAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        // Тестовая реализация - просто возвращает промпт как ответ
        return Task.FromResult("s");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> StreamAsync(
        AiRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Тестовая реализация - отдаёт последнее сообщение по словам, имитируя поток
        foreach (var word in request.Messages[^1].Content.Split(' '))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
            yield return word + " ";
        }
    }
}