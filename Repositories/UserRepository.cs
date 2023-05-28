using RestOrderService.Models;

namespace RestOrderService.Repositories;

public interface IUserRepository
{
    public Task<List<User>> FindAllUsers();
    
    public Task<User?> FindUserById(int id);
    
    public Task<User?> FindUserByNickname(string nickname);
    
    public Task<User?> FindUserByLogin(string login);
    
    public Task<User?> FindUserByToken(string token);

    public bool IsLoginValid(string login);
    
    public bool IsPasswordSafe(string password);

    public Task AddNewUser(User user);

    public Task UpdateUser(User user);
}