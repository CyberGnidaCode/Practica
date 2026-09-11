namespace Engine.Application.Messaging;

/// <summary>
/// Базовая обёртка обработчика, скрывающая конкретный тип запроса за известным типом ответа.
/// </summary>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
internal abstract class RequestHandlerWrapper<TResponse>
{
    /// <summary>
    /// Разрешает обработчик и поведения из контейнера и выполняет конвейер.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="provider">Поставщик сервисов для разрешения обработчика и поведений.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса.</returns>
    public abstract Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider provider, CancellationToken cancellationToken);
}
