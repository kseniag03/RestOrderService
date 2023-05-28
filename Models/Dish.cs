using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestOrderService.Models;

[Table("dish")]
public class Dish
{
    [Key]
    [Column("id")]
    public int Id { get; private set; }

    [Column("name")]
    public string Name { get; private set; } = string.Empty;

    [Column("description")]
    public string Description { get; private set; } = string.Empty;

    [Column("price")]
    public decimal Price { get; private set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    public Dish() { }

    public Dish(string name, string description, decimal price, int quantity)
    {
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
    }
}