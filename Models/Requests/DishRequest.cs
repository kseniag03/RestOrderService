namespace RestOrderService.Models.Requests;

/// <summary>
/// Class for representation of manager's input data (for adding new dish)
/// </summary>
public class DishRequest
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required decimal Price { get; set; }
    
    public required int Quantity { get; set; }
}