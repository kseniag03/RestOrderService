using RestOrderService.Libraries;

namespace RestOrderService.Models.Enums;

public enum OrderStatus
{
    [StringValue("pending")]
    Pending,
    
    [StringValue("inprogress")]
    InProgress,
    
    [StringValue("completed")]
    Completed,
    
    [StringValue("canceled")]
    Canceled
}
