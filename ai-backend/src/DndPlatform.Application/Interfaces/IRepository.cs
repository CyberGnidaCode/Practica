namespace DndPlatform.Application.Interfaces;

/// <summary>
/// Интерфейс репозитория.
/// </summary>
/// <typeparam name="TEntity">
/// Класс сущности.
/// </typeparam>
public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Получение всех сущностей.
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Коллекция всех сущностей
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Запрос на получение списка сущностей
    /// </summary>
    /// <returns>
    /// Запрос на получение списка сущностей.
    /// </returns>
    IQueryable<TEntity> GetAllQuery();

    /// <summary>
    /// Получение объекта по айди.
    /// </summary>
    /// <param name="id">Айди.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление нового объекта.
    /// </summary>
    /// <param name="entity">объект.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>
    /// Task
    /// </returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление объекта.
    /// </summary>
    /// <param name="entity">Объект.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Удаление объекта по айди.
    /// </summary>
    /// <param name="id">Айди.</param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>Количество удаленных.</returns>
    Task<int> RemoveAsync(Guid id, CancellationToken cancellationToken);
}