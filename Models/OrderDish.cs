using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestOrderService.Models;

/// <summary>
/// Model of object connecting order with dishes.
/// </summary>
[Table("order_dish")]
public class OrderDish
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    [ForeignKey("Order")]
    public int OrderId { get; set; }

    [Column("dish_id")]
    [ForeignKey("Dish")]
    public int DishId { get; set; }

    [Column("quantity")]
    public int Quantity { get; private set; }

    [Column("price")]
    public decimal Price { get; private set; }

    public OrderDish() { }

    public OrderDish(int dishId, int quantity, decimal price)
    {
        DishId = dishId;
        Quantity = quantity;
        Price = price;
    }
}