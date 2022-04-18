using Pdsr.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Data;

public class PdsrRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields
    private DbSet<TEntity>? _entities;
    private readonly IDbContext _dbContext;
    #endregion

    #region Ctor
    public PdsrRepositoryBase(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    #endregion

    #region Properties

    /// <summary>
    /// Gets a Table as <see cref="IAsyncEnumerable{T}" />
    /// </summary>
    public virtual IAsyncEnumerable<TEntity> TableAsync => Entities.AsAsyncEnumerable<TEntity>();

    /// <summary>
    /// Gets a Table as <see cref="IAsyncEnumerable{T}" with no tracking/>
    /// </summary>
    public virtual IAsyncEnumerable<TEntity> TableAsyncNoTracking => Entities.AsNoTracking().AsAsyncEnumerable();

    /// <summary>
    /// Gets a table
    /// </summary>
    public virtual IQueryable<TEntity> Table => Entities;

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    /// <summary>
    /// Gets an entity set
    /// </summary>
    protected virtual DbSet<TEntity> Entities
    {
        get
        {
            if (_entities == null)
                _entities = _dbContext.Set<TEntity>();

            return _entities;
        }
    }

    #endregion

    #region ErrorHandling
    protected virtual string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
    {
        if (_dbContext is DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries()
                .Where(ent => ent.State == EntityState.Added || ent.State == EntityState.Modified).ToList();
            entries.ForEach(entry => entry.State = EntityState.Unchanged);
        }

        _dbContext.SaveChanges();
        return exception.ToString();
    }

    protected virtual async Task<string> GetFullErrorTextAndRollbackEntityChangesAsync(DbUpdateException exception)
    {
        if (_dbContext is DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries()
                .Where(ent => ent.State == EntityState.Added || ent.State == EntityState.Modified).ToList();
            entries.ForEach(entry => entry.State = EntityState.Unchanged);
        }

        await _dbContext.SaveChangesAsync();
        return exception.ToString();
    }
    #endregion

    #region NormalCRUD Operations
    public virtual TEntity? GetById(object id) => Entities.Find(id);

    public virtual void Insert(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        try
        {
            Entities.Add(entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    public virtual void Insert(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.AddRange(entities);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    public virtual void Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Update(entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    public virtual void Update(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.UpdateRange(entities);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }


    public virtual void Delete(IEnumerable<TEntity> entities)
    {

        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.RemoveRange(entities);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }
    #endregion

    #region Async methods

    public virtual Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return TableNoTracking.FirstOrDefaultAsync(a => a.Id == (int)id, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            //_dbContext.Detach(entity);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.AddRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
            //foreach (var entity in entities)
            //    _dbContext.Detach(entity);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            //_dbContext.Detach(entity);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.UpdateRange(entities);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
            //foreach (var entity in entities)
            //    _dbContext.Detach(entity);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }



    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.RemoveRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task DeleteByIdAsync(int entityId, CancellationToken cancellationToken = default)
    {
        if (entityId == 0) throw new ArgumentOutOfRangeException(nameof(entityId));
        try
        {
            TEntity? entity = await GetByIdAsync(entityId, cancellationToken);
            if (entity == null) throw new ArgumentNullException(nameof(entityId));
            await DeleteAsync(entity, cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
        }
    }

    public virtual async Task DeleteByIdAsync(IEnumerable<int> entitiyIds, CancellationToken cancellationToken = default)
    {
        if (entitiyIds == null || !entitiyIds.Any()) throw new ArgumentNullException(nameof(entitiyIds));

        //var a = TableNoTracking.Where(e => entitiyIds.Contains(e.Id));
        //await DeleteAsync(a);
        var entities = ((IQueryable<TEntity>)Entities).Where(e => entitiyIds.Contains(e.Id));
        //var a = Entities.WhereAwait<TEntity>((e, v) => await entitiyIds.Contains(e.Id))
        //var entities = Entities.Where(e => entitiyIds.Contains(e.Id));
        await DeleteAsync(entities, cancellationToken);
    }


    #endregion
}
