using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using RestOrderService.Databases;
using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Services;

/// <summary>
/// Class for users management, implements IUserRepository interface.
/// </summary>
public class UserService: IUserRepository
{
    /// <summary>
    /// Database with tables (used user, session).
    /// </summary>
    private readonly DataContext _database;

    public UserService(DataContext database)
    { 
        _database = database;
    }

    public async Task<List<User>> FindAllUsers()
    {
        var users = await _database.Users.ToListAsync();
        return users;
    }

    public async Task<User?> FindUserById(int id)
    {
        var user = await _database.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> FindUserByNickname(string nickname)
    {
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
        return user;
    }
    
    public async Task<User?> FindUserByLogin(string login)
    {
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Email == login);
        return user;
    }

    public async Task<User?> FindUserByToken(string token)
    {
        var userSession = await _database.Sessions.FirstOrDefaultAsync(s => s.SessionToken == token);
        if (userSession == null)
        {
            return null;
        }
        var user = await _database.Users.FindAsync(userSession.UserId);
        return user;
    }

    public bool IsLoginValid(string login)
    {
        const string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        return Regex.IsMatch(login, emailPattern);
    }
    
    public bool IsPasswordSafe(string password)
    {
        if (password.Length < 5)
        {
            return false;
        }
        string[] patterns = {
            @"[\W_]",       // Check for at least one special symbol
            @"[A-Z]",        // Check for at least one uppercase letter
            @"[a-z]",        // Check for at least one lowercase letter
            @"\d"            // Check for at least one digit
        };
        return patterns.All(pattern => Regex.IsMatch(password, pattern));
    }
    
    public async Task AddNewUser(User user)
    {
        await _database.Users.AddAsync(user);
        await _database.SaveChangesAsync();
    }
    
    public async Task UpdateUser(User user)
    {
        _database.Users.Update(user);

        if (user.Token.Length > 0)
        {
            var parsedToken = new JwtSecurityToken(user.Token);
            var expirationDate = parsedToken.ValidTo;
            var session = new Session(user.Id, user.Token, expirationDate);
            _database.Sessions.Add(session);
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _database.SaveChangesAsync();
    }
}
