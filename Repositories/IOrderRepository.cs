using RestOrderService.Models;

namespace RestOrderService.Repositories;

/// <summary>
/// Interface for orders management,
/// connects OrderController with order, dish and order_dish table in database.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Searches for all orders in database order table.
    /// </summary>
    /// <returns> List of orders found </returns>
    public Task<List<Order>> FindAllOrders();
    
    /// <summary>
    /// Searches for order in database order table by its id.
    /// </summary>
    /// <param name="id"> Order's id </param>
    /// <returns> Found order, if it is present in db, otherwise <c>null</c> </returns>
    public Task<Order?> FindOrderById(int id);
    
    /// <summary>
    /// Adds order to database in order table,
    /// for every dish from dishes list updates dish.orderId field,
    /// launches order processing,
    /// updates database.
    /// </summary>
    /// <param name="order"> Current order </param>
    /// <param name="dishes"> Dishes ordered </param>
    public Task AddNewOrder(Order order, List<OrderDish> dishes);
    
    /// <summary>
    /// Searches for dish in database dish table by its id.
    /// </summary>
    /// <param name="id"> Dish's id </param>
    /// <returns> Found dish, if it is present in db, otherwise <c>null</c> </returns>
    public Task<Dish?> FindDishById(int id);
}