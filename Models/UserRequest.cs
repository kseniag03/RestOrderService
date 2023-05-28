namespace RestOrderService.Models;

public class UserRequest
{
    public required int Id { get; set; }
    
    public required String Nickname { get; set; }
    
    public required String Email { get; set; }
    
    public required String Password { get; set; }
}