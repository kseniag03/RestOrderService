namespace RestOrderService.Models;

public class User
{
    private int id;

    private String nickname;

    private String email;

    private String password;

    private Role role;
    
    public User()
    {
        
    }

    public User(int id, String nickname, String email, Role role)
    {
        this.id = id;
        this.nickname = nickname;
        this.email = email;
        this.role = role;
        
        Id = id;
        Nickname = nickname;
        Email = email;
        Role = role;
    }
    
    public int Id { get; private set; }
    public string Nickname { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Role Role { get; private set; }
    
    public string PasswordHash { get; set; }

    public string Token { get; set; } = string.Empty;

    public string Password
    {
        private get { return password; }
        set
        {
            // Implement your password validation logic here
            // For example, check minimum length, complexity, etc.
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Password cannot be empty");
            }

            password = value;
        }
    }
}