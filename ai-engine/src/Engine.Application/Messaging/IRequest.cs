namespace Engine.Application.Messaging;

/// <summary>
/// Маркерный интерфейс запроса (команды или запроса чтения), возвращающего ответ типа <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
public interface IRequest<out TResponse>
{
}
