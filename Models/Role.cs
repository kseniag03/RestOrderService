namespace RestOrderService.Models;

public enum Role
{
    [StringValue("Customer")]
    CUSTOMER,
    [StringValue("Chef")]
    CHEF,
    [StringValue("Manager")]
    MANAGER
}