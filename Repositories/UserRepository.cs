using RestOrderService.Models;

namespace RestOrderService.Repositories;

public interface IUserRepository
{
    public Task<User?> FindUserById(int id);
    
    public Task<User?> FindUserByNickname(string nickname);
    
    public Task<User?> FindUserByToken(string token);
}