using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Services;

public class OrderService: IOrderRepository
{
    private readonly DataContext _database;

    public OrderService(DataContext database)
    { 
        _database = database;
    }
    
    public async Task<Order?> FindOrderById(int id)
    {
        var order = await _database.Orders.FindAsync(id);
        return order;
    }
    
    public async Task<List<Order>> FindAllOrders()
    {
        var orders = await _database.Orders.ToListAsync();
        return orders;
    }

    public Task AddNewOrder(Order order)
    {
        throw new NotImplementedException();
    }
}