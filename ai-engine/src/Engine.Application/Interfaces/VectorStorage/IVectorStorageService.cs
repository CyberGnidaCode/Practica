namespace Engine.Application.Interfaces.VectorStorage;

/// <summary>
/// Интерфейс сервиса для работы с векторной БД
/// </summary>
public interface IVectorStorageService
{
    /// <summary>
    /// Текст
    /// </summary>
    /// <param name="text">
    /// Текст поиска
    /// </param>
    /// <param name="gameId">
    /// Идентификатор игры, в рамках которой выполняется поиск
    /// </param>
    /// <returns>
    /// result
    /// </returns>
    Task<List<string>> SearchAsync(string text, Guid gameId);

    /// <summary>
    /// Перованачальная настройка коллекций
    /// </summary>
    /// <returns>
    /// Task
    /// </returns>
    Task SetColletionsAsync();

    /// <summary>
    /// Добавляет точку в коллекцию.
    /// </summary>
    /// <param name="text">Текст для добавления</param>
    /// <param name="gameId">Идентификатор игры</param>
    /// <returns>Task</returns>
    Task AddPointAsync(string text, Guid gameId);
}