using Pdsr.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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
    /// creates a LINQ query
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <param name="sql"></param>
    /// <returns></returns>
    //    IQueryable<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class;

    /// <summary>
    /// returns a set of queryable data by raw sql command
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="sql"></param>
    /// <returns></returns>
    //     IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity;

    /// <summary>
    /// executes a raw sql command
    /// </summary>
    /// <param name="sqlString"></param>
    /// <param name="doNotEnsureTransaction"></param>
    /// <param name="timeout"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    // int ExecuteSqlCommand(RawSqlString sqlString, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

    /// <summary>
    /// Detachs an entity from context
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
}
