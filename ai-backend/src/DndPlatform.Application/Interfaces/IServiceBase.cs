namespace DndPlatform.Application.Interfaces;

/// <summary>
/// Интерфейс базового сервиса
/// </summary>
/// <typeparam name="TEntity">
/// Сущность
/// </typeparam>
public interface IServiceBase<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Получение всех сущностей
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Коллекция всех сущностей
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    /// <param name="id">
    /// Идентификатор
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Сущность
    /// </returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление сущности в бд
    /// </summary>
    /// <param name="entity">
    /// Сущность
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Кол-во добавленных сущностей
    /// </returns>
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление сущности в бд
    /// </summary>
    /// <param name="entity">
    /// Сущность
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Кол-во обновленных сущностей
    /// </returns>
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сущности из бд по идентификатору
    /// </summary>
    /// <param name="id">
    /// Идентификатор
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Кол-во удаленных сущностей
    /// </returns>
    Task<int> RemoveAsync(Guid id, CancellationToken cancellationToken);
}