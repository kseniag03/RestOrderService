using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestOrderService.Models.Enums;

namespace RestOrderService.Models;

/// <summary>
/// Model of user object.
/// </summary>
[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]
    public string Nickname { get; private set; } = string.Empty;
    
    [Column("email")]
    public string Email { get; private set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; private set; } = string.Empty;

    [Column("role")]
    public Role Role { get; set; } = Role.Customer;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [NotMapped]
    public string Token { get; set; } = string.Empty;

    public User() {}

    public User(string nickname, string email, string passwordHash, Role role)
    {
        Nickname = nickname;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}