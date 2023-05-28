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
    public async Task<User?> FindUserById(int id)
    {
        var user = await _database.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> FindUserByNickname(string nickname)
    {
        var user = await _database.Users.FindAsync(nickname);
        return user;
    }

    public async Task<User?> FindUserByToken(string token)
    {
        var user = await _database.Users.FindAsync(token);
        return user;
    }
}