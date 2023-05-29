using RestOrderService.Models;
using RestOrderService.Models.Enums;
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

    public async Task AddNewOrder(Order order, List<OrderDish> dishes)
    {
        await _database.Orders.AddAsync(order);
        await _database.SaveChangesAsync();
        _database.Orders.Update(order);
        
        foreach (var dish in dishes)
        {
            dish.OrderId = order.Id;
            _database.OrderDishes.Update(dish);
            await _database.SaveChangesAsync();
        }

        order = await ProcessOrder(order);
        order.UpdatedAt = DateTime.UtcNow;
        
        _database.Orders.Update(order);
        await _database.SaveChangesAsync();
    }
    
    private async Task<Order> ProcessOrder(Order order)
    {
        order.Status = OrderStatus.InProgress;
        await Task.Delay(15000);
        order.Status = OrderStatus.Completed;
        return order;
    }
    
    public async Task<Dish?> FindDishById(int id)
    {
        var dish = await _database.Dishes.FirstOrDefaultAsync(u => u.Id == id);
        return dish;
    }

}