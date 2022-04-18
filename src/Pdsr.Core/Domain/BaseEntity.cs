using System;

namespace Pdsr.Core.Domain;

/// <summary>
/// Base Entity
/// All derived children are having equality compare based on Id field.
/// override <see cref="EqualsCore(BaseEntity)"/> and <see cref="GetHashCodeCore"/> to change the contract
/// </summary>
public abstract record BaseEntity : IEquatable<BaseEntity>, IBaseEntity
{
    public int Id { get; set; }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified BaseEntity
    ///     value.
    /// </summary>
    /// <param name="other">other object to test with</param>
    /// <returns> true if obj has the same value as this instance; otherwise, false.</returns>
    public virtual bool Equals(
#if NETSTANDARD2_0 || NETSTANDARD2_1
        BaseEntity
#else
        BaseEntity?
#endif
        other) => EqualsCore(other);


    /// <summary>
    /// Returns the hash code for this instance based on Id.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => GetHashCodeCore();

    /// <summary>
    /// Equals method suitable to override in children classes
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected virtual bool EqualsCore(object? other)
    {
        if (other == null)
            return false;
        var otherBaseEntity = other as BaseEntity;
        if (otherBaseEntity == null)
            return false;
        else
            return this.Id.Equals(otherBaseEntity.Id);
    }

    /// <summary>
    /// GetHashCode, suitable to override in child records
    /// </summary>
    /// <returns></returns>
    protected virtual int GetHashCodeCore()
    {
        return this.Id.GetHashCode();
    }
}




public abstract record class BaseEntity<TKey> : IBaseEntity<TKey>
    where TKey : notnull
{
    public BaseEntity(TKey id)
    {
        Id = id;
    }

    public TKey Id { get; set; }
}
