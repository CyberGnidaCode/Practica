namespace Engine.Application.Messaging;

/// <summary>
/// Отправляет запросы соответствующему обработчику через конвейер поведений.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Отправляет запрос его обработчику и возвращает результат.
    /// </summary>
    /// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
