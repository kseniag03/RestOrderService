namespace RestOrderService.Models;

public class UserRequest
{
    public required String Nickname { get; set; }
    
    public required String Login { get; set; }
    
    public required String Password { get; set; }
}