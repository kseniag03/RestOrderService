using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestOrderService.Models;

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
    public int DishId { get; private set; }

    [Column("quantity")]
    public int Quantity { get; private set; }

    [Column("price")]
    public decimal Price { get; private set; }

    public OrderDish() { }

    public OrderDish(int orderId, int dishId, int quantity, decimal price)
    {
        OrderId = orderId;
        DishId = dishId;
        Quantity = quantity;
        Price = price;
    }
}