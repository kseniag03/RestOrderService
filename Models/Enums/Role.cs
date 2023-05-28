using RestOrderService.Libraries;

namespace RestOrderService.Models;

public enum Role
{
    [StringValue("customer")]
    Customer,
    [StringValue("chef")]
    Chef,
    [StringValue("manager")]
    Manager
}