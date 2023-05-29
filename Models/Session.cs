using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestOrderService.Models;

[Table("session")]
public class Session
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Column("session_token")]
    public string SessionToken { get; set; } = string.Empty;

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    public Session() { }

    public Session(int userId, string sessionToken, DateTime expiresAt)
    {
        UserId = userId;
        SessionToken = sessionToken;
        ExpiresAt = expiresAt;
    }
}