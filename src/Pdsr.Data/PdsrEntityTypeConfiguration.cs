using Pdsr.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pdsr.Data;

/// <summary>
/// Data Type Configuration for mappings
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class PdsrEntityTypeConfiguration<TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// for overriding later
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void PostConfiguration(EntityTypeBuilder<TEntity> builder) { }

    /// <summary>
    /// Config the entity
    /// </summary>
    /// <param name="builder"></param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        PostConfiguration(builder);
    }

    /// <summary>
    /// Apply mapping
    /// </summary>
    /// <param name="modelBuilder"></param>
    public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(this);
    }
}


/// <summary>
/// Data Type Configuration for mappings
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey">Type of entity Key</typeparam>
public class PdsrEntityTypeConfiguration<TKey, TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// for overrideing later
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void PostConfiguration(EntityTypeBuilder<TEntity> builder) { }

    /// <summary>
    /// Config the entity
    /// </summary>
    /// <param name="builder"></param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        PostConfiguration(builder);
    }

    /// <summary>
    /// Apply mapping
    /// </summary>
    /// <param name="modelBuilder"></param>
    public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(this);
    }
}
