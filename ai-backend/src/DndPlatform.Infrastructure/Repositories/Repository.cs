namespace DndPlatform.Infrastructure.Repositories;

using DndPlatform.Application.Interfaces;
using DndPlatform.Persistence;
using DndPlatform.Persistence.Base;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Базовый репозиторий сущностей
/// </summary>
/// <typeparam name="TEntity">
/// Сущность
/// </typeparam>
public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Конектст
    /// </summary>
    private readonly StorageContext context;

    /// <summary>
    /// Базовый репозиторий сущностей
    /// </summary>
    /// <param name="context">
    /// Конектст
    /// </param>
    public Repository(StorageContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Получить список сущностей
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await this.context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получить список сущностей
    /// </summary>
    /// <returns>
    /// Список сущностей
    /// </returns>
    public IQueryable<TEntity> GetAllQuery()
    {
        return this.context.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    /// <summary>
    /// Получить сущность по id
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
        return await this.context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <summary>
    /// Добавить сущность
    /// </summary>
    /// <param name="entity">
    /// Сущность
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены
    /// </param>
    /// <returns>
    /// Task
    /// </returns>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await this.context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Удалить сущность
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
        return await this.context.Set<TEntity>().Where(e => e.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Обновление сущности
    /// </summary>
    /// <param name="entity">
    /// Сущность
    /// </param>
    public void Update(TEntity entity)
    {
        this.context.Set<TEntity>().Update(entity);
    }
}