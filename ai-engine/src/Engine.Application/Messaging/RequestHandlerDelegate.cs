namespace Engine.Application.Messaging;

/// <summary>
/// Делегат следующего шага конвейера обработки запроса.
/// </summary>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
/// <returns>Результат следующего шага конвейера.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
