using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestOrderService.Models.Enums;

namespace RestOrderService.Models;

/// <summary>
/// Model of order object.
/// </summary>
[Table("order")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; private set; }

    [Column("status")]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Column("special_requests")]
    public string SpecialRequests { get; private set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Order() { }

    public Order(int userId, string specialRequests)
    {
        UserId = userId;
        SpecialRequests = specialRequests;
    }
}