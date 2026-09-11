namespace DndPlatform.Infrastructure.Servises;

using DndPlatform.Application.Interfaces;
using DndPlatform.Persistence;
using DndPlatform.Persistence.Base;

/// <summary>
/// Базовый сервис сущностей
/// </summary>
/// <typeparam name="TEntity">
/// Сущность
/// </typeparam>
public class ServiceBase<TEntity> : IServiceBase<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Контекст
    /// </summary>
    private readonly StorageContext context;

    /// <summary>
    /// Репозиторий сущности
    /// </summary>
    private readonly IRepository<TEntity> repository;

    /// <summary>
    /// Базовый сервис сущностей
    /// </summary>
    /// <param name="context">
    /// Контекст
    /// </param>
    /// <param name="repository">
    /// Репозиторий сущности
    /// </param>
    public ServiceBase(StorageContext context, IRepository<TEntity> repository)
    {
        this.context = context;
        this.repository = repository;
    }

    /// <summary>
    /// Получение всех сущностей
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Коллекция всех сущностей
    /// </returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        // get
        return await this.repository.GetAllAsync(cancellationToken);
    }

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
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // get
        return await this.repository.GetByIdAsync(id, cancellationToken);
    }

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
    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        // add
        await this.repository.AddAsync(entity, cancellationToken);

        var result = await this.context.SaveChangesAsync(cancellationToken);

        // return
        return result;
    }

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
    public async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        // date update
        entity.DateUpdate = DateTime.UtcNow;

        // update
        this.repository.Update(entity);

        var result = await this.context.SaveChangesAsync(cancellationToken);

        // return
        return result;
    }

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
    public async Task<int> RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        // remove
        return await this.repository.RemoveAsync(id, cancellationToken);
    }
}