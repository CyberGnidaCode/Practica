namespace Engine.Application.Messaging;

/// <summary>
/// Реализация <see cref="ISender"/> поверх контейнера зависимостей.
/// </summary>
internal sealed class Sender : ISender
{
    /// <summary>
    /// Обертка для вызова обработчика запроса без знания конкретных типов запроса и ответа.
    /// Реализует интерфейс <see cref="RequestHandlerWrapper{TResponse}"/>, который позволяет вызвать метод Handle.
    /// </summary>
    private static readonly Type WrapperOpenType = typeof(RequestHandlerWrapperImpl<,>);

    /// <summary>
    /// Провайдер сервисов для разрешения обработчиков запросов и их зависимостей.
    /// Используется внутри обертки для получения экземпляра обработчика запроса.
    /// Не сохраняет в себе конкретных обработчиков, что позволяет работать с любыми типами запросов и ответов.
    /// </summary>
    private readonly IServiceProvider provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Sender"/> class.
    /// </summary>
    /// <param name="provider">Поставщик сервисов.</param>
    public Sender(IServiceProvider provider) => this.provider = provider;

    /// <inheritdoc />
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wrapperType = WrapperOpenType.MakeGenericType(request.GetType(), typeof(TResponse));

        var instance = Activator.CreateInstance(wrapperType);

        var wrapper = (RequestHandlerWrapper<TResponse>)instance!;

        return wrapper.Handle(request, this.provider, cancellationToken);
    }
}
