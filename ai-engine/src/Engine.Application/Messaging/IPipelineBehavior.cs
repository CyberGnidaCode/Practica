namespace Engine.Application.Messaging;

/// <summary>
/// Шаг конвейера, выполняемый вокруг обработки запроса (например, валидация или логирование).
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса.</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Выполняет шаг конвейера и при необходимости вызывает следующий шаг.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="nextHandler">Делегат следующего шага конвейера.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса.</returns>
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> nextHandler, CancellationToken cancellationToken);
}
