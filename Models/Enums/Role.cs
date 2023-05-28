using RestOrderService.Libraries;

namespace RestOrderService.Models.Enums;

public enum Role
{
    [StringValue("customer")]
    Customer,
    [StringValue("chef")]
    Chef,
    [StringValue("manager")]
    Manager
}