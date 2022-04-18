namespace Pdsr.Core.Domain;

/// <summary>
/// all inherited objects has interface
/// </summary>
public interface ISubjectOwnable
{
    /// <summary>
    /// user's subject id
    /// </summary>
    public string SubjectId { get; init; }
}
