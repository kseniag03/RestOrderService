namespace RestOrderService.Models.Requests;

/// <summary>
/// Class for representation of user's input data (for registration and login)
/// </summary>
public class UserRequest
{
    public required string Nickname { get; set; }
    
    public required string Login { get; set; }
    
    public required string Password { get; set; }
}