namespace Pdsr.Core.User;

/// <summary>
/// Base Session while new scope being created for a user,
/// or while User's background tasks starts.
/// for more infor <see cref="ISubjectOwnerProvider"/>
/// </summary>
public interface ISessionBase
{
    /// <summary>
    /// User's Subject Id
    /// </summary>
    string? SubjectId { get; set; }

    /// <summary>
    /// Set subject id for current session
    /// </summary>
    /// <param name="subjectId">Subject Id</param>
    /// <param name="runningWithoutUser">Indicate if this is running in background of through actual user</param>
    void SetSubjectId(string subjectId, bool runningWithoutUser = true);

    /// <summary>
    /// Setups a session and returns an instance of <see cref="ISessionBase"/> to store into DI container.
    /// </summary>
    /// <param name="subjectId">user sub</param>
    /// <param name="nonInteractively">if the session going to be run in the background</param>
    /// <returns>A created instance of session</returns>
    ISessionBase Setup(string subjectId, bool nonInteractively = true);

    /// <summary>
    /// Setups a session and returns an instance of <typeparamref name="TSession"/>
    /// </summary>
    /// <typeparam name="TSession">The type of Session, implementing <see cref="ISessionBase"/></typeparam>
    /// <param name="subjectId">User's subject id</param>
    /// <param name="nonInteractively">if the session going to be run in the background</param>
    /// <returns>A created instance of session</returns>
    TSession Setup<TSession>(string subjectId, bool nonInteractive = true) where TSession : ISessionBase;
}
