using System.Diagnostics;

namespace Pdsr.Core.Domain;

/// <summary>
/// Entity base for the user ownnable objects
/// </summary>
/// <typeparam name="TKey">Type of Id to use in this entity</typeparam>
public abstract record class SubjectOwnedEntityBase<TKey> : BaseEntity<TKey>, ISubjectOwnable
    where TKey : notnull
{
    public SubjectOwnedEntityBase(TKey id, string subjectId) : base(id)
    {
        SubjectId = subjectId;
    }

    /// <summary>
    /// The subject id of the user owning this entity
    /// </summary>
    public string SubjectId { get; init; }
}

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract record class UserSubjectOwnedEntityBase : SubjectOwnedEntityBase<string>, ISubjectOwnable
{
    public UserSubjectOwnedEntityBase(string id, string subjectId) : base(id, subjectId)
    {

    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
