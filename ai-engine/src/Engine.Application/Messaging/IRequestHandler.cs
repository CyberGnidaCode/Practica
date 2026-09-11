namespace Engine.Application.Messaging;

/// <summary>
/// Обработчик запроса <typeparamref name="TRequest"/>, возвращающего ответ <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса.</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Обрабатывает запрос.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
