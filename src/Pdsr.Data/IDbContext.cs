using Microsoft.EntityFrameworkCore;
using Pdsr.Core.Domain;

namespace Pdsr.Data;

public interface IDbContext
{
    /// <summary>
    /// Creates a DbSet that can be used to query and save instances of entity
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A set for the given entity type</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a script to create all tables for the current model
    /// </summary>
    /// <returns>A SQL script</returns>
    string GenerateCreateScript();

    /// <summary>
    /// Detachs an entity from context
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
}
