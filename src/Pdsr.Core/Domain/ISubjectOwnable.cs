namespace Pdsr.Core.Domain;

/// <summary>
/// all inherited objects has
/// </summary>
public interface ISubjectOwnable
{
    /// <summary>
    /// user's subject id
    /// </summary>
    public string SubjectId { get; init; }
}
