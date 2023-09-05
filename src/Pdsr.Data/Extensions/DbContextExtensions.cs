using Microsoft.EntityFrameworkCore;
using Pdsr.Core.Domain;

namespace Pdsr.Data.Extensions;

public static class DbContextExtensions
{

    /// <summary>
    /// Deatches a entity from a DbContext
    /// </summary>
    /// <typeparam name="TEntity">Entity Type drived from <see cref="BaseEntity"/></typeparam>
    /// <param name="context">DbContext to detach entity from</param>
    /// <param name="entity">Entity to detach</param>
    public static void Detach<TEntity>(this DbContext context, TEntity entity)
        where TEntity : notnull, BaseEntity
    {
        var local = context.Set<TEntity>().Local.FirstOrDefault(e => entity.Id.Equals(e.Id));
        if (local != null)
        {
            context.Entry(entity).State = EntityState.Detached;
        }
    }

    public static void Detach<TKey, TEntity>(this DbContext context, TEntity entity)
        where TEntity : BaseEntity<TKey>
        where TKey : notnull
    {
        var local = context.Set<TEntity>().Local.FirstOrDefault(e => entity.Id.Equals(e.Id));
        if (local != null)
        {
            context.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Detaches a collection of entities
    /// </summary>
    /// <typeparam name="TEntity">Entity Type drived from <see cref="BaseEntity"/></typeparam>
    /// <param name="context">DbContext to detach entitites from</param>
    /// <param name="entities">Entities to detach</param>
    public static void Detach<TEntity>(this DbContext context, IEnumerable<TEntity> entities)
        where TEntity : BaseEntity
    {
        foreach (var entity in entities)
        {
            var local = context.Set<TEntity>().Local.FirstOrDefault(e => entity.Id.Equals(e.Id));

            if (local is not null)
            {
                context.Entry<BaseEntity>(entity).State = EntityState.Detached;
            }
        }
    }

    public static void Detach<TKey, TEntity>(this DbContext context, IEnumerable<TEntity> entities)
        where TEntity : BaseEntity<TKey>
        where TKey : notnull
    {
        foreach (var entity in entities)
        {
            var local = context.Set<TEntity>().Local.FirstOrDefault(e => entity.Id.Equals(e.Id));

            if (local is not null)
            {
                context.Entry<BaseEntity<TKey>>(entity).State = EntityState.Detached;
            }
        }
    }
}
