using Pdsr.Core.Domain;

namespace Pdsr.Core.User;

/// <summary>
/// The subject owner provider
/// </summary>
public interface ISubjectOwnerProvider : ISessionBase
{
    /// <summary>
    /// User has subject Id.
    /// Each time a session is being created, the subject id must be set.
    /// </summary>
    /// <returns></returns>
    bool HasSubjectId();

    /// <summary>
    /// User's Jwt AccessToken
    /// </summary>
    string? JwtToken { get; set; }

    /// <summary>
    /// While running in background as service with no user involved.
    /// Running Non Interactively
    /// </summary>
    bool RunningNonInteractively { get; set; }

    /// <summary>
    /// Set subject id in session based on a ISubjectOwnable item, ie: Session
    /// In other words, it reads an obect, i.e. from database and setups the <see cref="RunningNonInteractively"/> session based on that object
    /// </summary>
    /// <returns>returns Subject Id of the user</returns>
    string SetSubjectIdForCurrentScope(ISubjectOwnable item);

    /// <summary>
    /// Throws Exception if subject id doesn't exists. Ensures the session validation.
    /// for some services load data depends on subject id, this can make it error 
    /// </summary>
    void ThrowIfSubjectIsNotSet();

    /// <summary>
    /// Allows this instance to exist without session id.
    /// </summary>
    bool AllowEmptySession { get; }
}
