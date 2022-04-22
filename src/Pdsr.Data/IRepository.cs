using Pdsr.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Data;

/// <summary>
/// Data Repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public interface IRepository<TEntity> where TEntity : IBaseEntity
{
    #region Methods

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    TEntity? GetById(object id);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    void Insert(TEntity entity);

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    void Insert(IEnumerable<TEntity> entities);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    void Update(TEntity entity);

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    void Update(IEnumerable<TEntity> entities);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    void Delete(IEnumerable<TEntity> entities);

    #endregion

    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    IQueryable<TEntity> TableNoTracking { get; }

    IAsyncEnumerable<TEntity> TableAsync { get; }

    IAsyncEnumerable<TEntity> TableAsyncNoTracking { get; }

    #endregion

    #region Async Methods
    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Entity</returns>
    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken"></param>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken"></param>
    Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete entiity by Id
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteByIdAsync(int entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete entities by Ids
    /// </summary>
    /// <param name="entitiyIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteByIdAsync(IEnumerable<int> entitiyIds, CancellationToken cancellationToken = default);
    #endregion
}
