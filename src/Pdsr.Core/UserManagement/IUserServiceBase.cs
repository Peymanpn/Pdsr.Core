using Pdsr.Core.Domain;

namespace Pdsr.Core.User;

/// <summary>
/// Base service for manage Users
/// </summary>
/// <typeparam name="TUser">an SubjectId user object <see cref="ISubjectOwnable"/></typeparam>
public interface IUserServiceBase<TUser> : IUserServiceBase<string, TUser>
    where TUser : PdsrUserBase<string>, ISubjectOwnable
{

}

/// <summary>
/// Base service for manage Users
/// </summary>
/// <typeparam name="TUser">an SubjectId user object <see cref="ISubjectOwnable"/></typeparam>
/// <typeparam name="TKey">Type of entity Key</typeparam>
public interface IUserServiceBase<TKey, TUser>
    where TUser : PdsrUserBase<TKey>, ISubjectOwnable
    where TKey : notnull
{
    /// <summary>
    /// Gets the user
    /// </summary>
    /// <returns>Returns the user</returns>
    TUser? GetUser();

    /// <summary>
    /// Gets the User asynchronously
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TUser?> GetUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the user of type <typeparamref name="TUser"/>
    /// </summary>
    /// <param name="user">The user to update</param>
    /// <param name="cancellation"></param>
    /// <returns>Returns the updated user</returns>
    Task<TUser> UpdateUser(TUser user, CancellationToken cancellation = default);

    /// <summary>
    /// Indicates if the user is valid (based on specific application's rules)
    /// </summary>
    /// <returns>Returns true if user is valid</returns>
    Task<bool> IsUserValid(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user with specified subject id.
    /// it checks for user existance before creating.
    /// If user already exists, nothing will happen
    /// </summary>
    /// <param name="subjectId">The Subject Identifier of the user</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns true if the user being created, otherwise false.</returns>
    Task<bool> CreateNewUserIfNotExists(string subjectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets collection of users Ids
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<TKey>> GetAllUsersIndex(CancellationToken cancellationToken = default);



    /// <summary>
    /// Usually called by the underlying service to disable or enable user in case of consequent errors, or multiple authentication failures.
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetUserEnableStatus(bool enable, CancellationToken cancellationToken = default);
}
