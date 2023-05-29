using RestOrderService.Databases;
using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Services;

/// <summary>
/// Class for dishes management, implements IDishRepository interface.
/// </summary>
public class DishService: IDishRepository
{
    /// <summary>
    /// Database with tables (used dish).
    /// </summary>
    private readonly DataContext _database;

    public DishService(DataContext database)
    {
        _database = database;
    }

    public async Task<Dish?> FindDishById(int id)
    {
        var dish = await _database.Dishes.FindAsync(id);
        return dish;
    }

    public async Task<Dish?> FindDishByName(string name)
    {
        var dish = await _database.Dishes.FirstOrDefaultAsync(u => u.Name == name);
        return dish;
    }

    public async Task<List<Dish>> FindAllDishes()
    {
        var dishes = await _database.Dishes.ToListAsync();
        return dishes;
    }

    public async Task AddNewDish(Dish dish)
    {
        await _database.Dishes.AddAsync(dish);
        await _database.SaveChangesAsync();
    }
    
    public async Task UpdateDish(Dish dish)
    {
        _database.Dishes.Update(dish);
        await _database.SaveChangesAsync();
    }

    public async Task DeleteDish(Dish dish)
    {
        _database.Dishes.Remove(dish);
        await _database.SaveChangesAsync();
    }
}