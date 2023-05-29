using RestOrderService.Models;

namespace RestOrderService.Repositories;

/// <summary>
/// Interface for dishes management,
/// connects DishController with dish table in database.
/// </summary>
public interface IDishRepository
{
    /// <summary>
    /// Searches for all dishes in database dish table.
    /// </summary>
    /// <returns> List of dishes found </returns>
    public Task<List<Dish>> FindAllDishes();
    
    /// <summary>
    /// Searches for dish in database dish table by its id.
    /// </summary>
    /// <param name="id"> Dish's id </param>
    /// <returns> Found dish, if it is present in db, otherwise <c>null</c> </returns>
    public Task<Dish?> FindDishById(int id);

    /// <summary>
    /// Searches for dish in database dish table by its name.
    /// </summary>
    /// <param name="name"> Dish's name </param>
    /// <returns> Found dish, if it is present in db, otherwise <c>null</c> </returns>
    public Task<Dish?> FindDishByName(string name);
    
    /// <summary>
    /// Adds dish to database in dish table,
    /// updates database.
    /// </summary>
    /// <param name="dish"> Current dish </param>
    public Task AddNewDish(Dish dish);

    /// <summary>
    /// Updates dish's fields in database in dish table,
    /// updates database.
    /// </summary>
    /// <param name="dish"> Current dish </param>
    public Task UpdateDish(Dish dish);

    /// <summary>
    /// Removes dish from database dish table,
    /// updates database.
    /// </summary>
    /// <param name="dish"> Current dish </param>
    public Task DeleteDish(Dish dish);
}
