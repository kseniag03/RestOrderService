using System.Text.RegularExpressions;
using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Services;

public class UserService: IUserRepository
{
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
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Token == token);
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
        _database.Users.Add(user);
        await _database.SaveChangesAsync();
    }
    
    public async Task UpdateUser(User user)
    {
        _database.Users.Update(user);
        await _database.SaveChangesAsync();
    }
}
