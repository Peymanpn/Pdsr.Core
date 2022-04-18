using System.Diagnostics;

namespace Pdsr.Core.Domain;

/// <summary>
/// Any entity which can belong to a user with <see cref="SubjectId"/>.
/// </summary>
public interface ISubjectOwnedEntityBase
{
    public string SubjectId { get; set; }
}


[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract record class UserSubjectOwnedEntityBase : ISubjectOwnedEntityBase
{
    public UserSubjectOwnedEntityBase(string subjectId) => SubjectId = subjectId;

    public string SubjectId { get; set; }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
