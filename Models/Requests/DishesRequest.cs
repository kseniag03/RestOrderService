namespace RestOrderService.Models.Requests;

/// <summary>
/// Class for representation of user's input data (for creating new order)
/// </summary>
public class DishesRequest
{
    public List<int> DishesIdList { get; set; } = new();
}