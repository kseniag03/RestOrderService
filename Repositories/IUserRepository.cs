using RestOrderService.Models;

namespace RestOrderService.Repositories;

/// <summary>
/// Interface for users management,
/// connects AuthController with user and session table in database.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Searches for all users in database user table.
    /// </summary>
    /// <returns> List of users found </returns>
    public Task<List<User>> FindAllUsers();
    
    /// <summary>
    /// Searches for user in database user table by its id.
    /// </summary>
    /// <param name="id"> User's id </param>
    /// <returns> Found user, if it is present in db, otherwise <c>null</c> </returns>
    public Task<User?> FindUserById(int id);
    
    /// <summary>
    /// Searches for user in database user table by its nickname (that is unique).
    /// </summary>
    /// <param name="nickname"> User's nickname </param>
    /// <returns> Found user, if it is present in db, otherwise <c>null</c> </returns>
    public Task<User?> FindUserByNickname(string nickname);
    
    /// <summary>
    /// Searches for user in database user table by its login (email).
    /// </summary>
    /// <param name="login"> User's login (email) </param>
    /// <returns> Found user, if it is present in db, otherwise <c>null</c> </returns>
    public Task<User?> FindUserByLogin(string login);
    
    /// <summary>
    /// Searches for userId in database session table by its token,
    /// searches for user in database user table by found userId.
    /// </summary>
    /// <param name="token"> User's token </param>
    /// <returns> Found user, if it is present in db, otherwise <c>null</c> </returns>
    public Task<User?> FindUserByToken(string token);

    /// <summary>
    /// Checks if email is valid:
    /// it must contain @ and domain name.
    /// </summary>
    /// <param name="login"> User's login (email) </param>
    /// <returns> <c>true</c>, if email is valid, otherwise <c>false</c> </returns>
    public bool IsLoginValid(string login);
    
    /// <summary>
    /// Checks if password is safe:
    /// its length must be 5 or more,
    /// it must contain at least one special symbol, uppercase and lowercase letter and digit.
    /// </summary>
    /// <param name="password"> Password for check </param>
    /// <returns> <c>true</c>, if password is appropriate, otherwise <c>false</c> </returns>
    public bool IsPasswordSafe(string password);

    /// <summary>
    /// Adds user to database in user table,
    /// updates database.
    /// </summary>
    /// <param name="user"> Current user </param>
    public Task AddNewUser(User user);

    /// <summary>
    /// Updates user's fields in database in user table after authorization,
    /// creates record of current session in database in session table,
    /// updates database.
    /// </summary>
    /// <param name="user"> Current user </param>
    public Task UpdateUser(User user);
}