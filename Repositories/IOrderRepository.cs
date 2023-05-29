using RestOrderService.Models;

namespace RestOrderService.Repositories;

public interface IOrderRepository
{
    public Task<Order?> FindOrderById(int id);

    public Task<List<Order>> FindAllOrders();

    public Task AddNewOrder(Order order, List<OrderDish> dishes);
    
    public Task<Dish?> FindDishById(int id);
}