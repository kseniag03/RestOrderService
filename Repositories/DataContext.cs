using RestOrderService.Models;

namespace RestOrderService.Repositories;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql("Host=localhost; Database=ros; Username=postgres; Password=_4a^h%7AF$v");
    }

    public DbSet<User> Users => Set<User>();
}