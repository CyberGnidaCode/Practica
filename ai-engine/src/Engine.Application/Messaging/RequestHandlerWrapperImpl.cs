namespace Engine.Application.Messaging;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Типизированная обёртка, выстраивающая конвейер поведений вокруг обработчика запроса.
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса.</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
internal sealed class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public override Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider provider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(provider);

        var handler = provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        Task<TResponse> Handler() => handler.Handle((TRequest)request, cancellationToken);

        RequestHandlerDelegate<TResponse> next = Handler;

        // оборачиваем обработчик поведениями в обратном порядке, чтобы первый зарегистрированный выполнялся первым
        var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse();

        foreach (var behavior in behaviors)
        {
            var previous = next;
            next = () => behavior.Handle((TRequest)request, previous, cancellationToken);
        }

        return next();
    }
}
