using RestOrderService.Models;

namespace RestOrderService.Databases;

/// <summary>
/// Representation of PostgresSQL database.
/// </summary>
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql("Host=localhost; Database=ros; Username=postgres; Password=_4a^h%7AF$v");
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Session> Sessions => Set<Session>();
    
    public DbSet<Dish> Dishes => Set<Dish>();
    
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderDish> OrderDishes => Set<OrderDish>();
}