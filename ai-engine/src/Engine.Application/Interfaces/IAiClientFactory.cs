namespace Engine.Application.Interfaces;

using Engine.Application.Enums;

/// <summary>
/// Фабрика для создания клиентов AI-сервисов
/// </summary>
public interface IAiClientFactory
{
    /// <summary>
    /// Создает экземпляр клиента AI-сервиса на основе указанного провайдера
    /// </summary>
    /// <param name="provider">
    /// Провайдер AI-сервиса
    /// </param>
    /// <returns>
    /// Экземпляр клиента AI-сервиса
    /// </returns>
    IAiClient Create(AiProvider provider);
}