using RestOrderService.Models;

namespace RestOrderService.Repositories;

public interface IDishRepository
{
    public Task<Dish?> FindDishById(int id);
    
    public Task<List<Dish>> FindAllDishes();

    public Task AddNewDish(Dish dish);

    public Task UpdateDish(Dish dish);

    public Task DeleteDish(Dish dish);
}
