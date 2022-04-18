namespace Pdsr.Core.Domain;

/// <summary>
/// User base
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IBaseEntity<TKey>
    where TKey : notnull
{
    TKey Id { get; set; }
}

/// <summary>
/// User base with integer as primary key
/// </summary>
public interface IBaseEntity : IBaseEntity<int>
{

}
