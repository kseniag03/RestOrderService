using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestOrderService.Models;

[Table("order_dish")]
public class OrderDish
{
    [Key]
    [Column("id")]
    public int Id { get; private set; }

    [Column("order_id")]
    [ForeignKey("Order")]
    public int OrderId { get; private set; }

    [Column("dish_id")]
    [ForeignKey("Dish")]
    public int DishId { get; private set; }

    [Column("quantity")]
    public int Quantity { get; private set; }

    [Column("price")]
    public decimal Price { get; private set; }

    public OrderDish() { }

    public OrderDish(int id, int orderId, int dishId, int quantity, decimal price)
    {
        Id = id;
        OrderId = orderId;
        DishId = dishId;
        Quantity = quantity;
        Price = price;
    }
}