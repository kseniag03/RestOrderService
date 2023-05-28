using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestOrderService.Models;
using RestOrderService.Repositories;
using RestOrderService.Services;

namespace RestOrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// Регистратор сообщений и ошибок.
    /// </summary>
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Хранилище данных о пользователях.
    /// </summary>
    private readonly IUserRepository _userRepository;
    
    public AuthController(IConfiguration configuration, IUserRepository userRepository, [FromServices]ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserRequest request)
    {
        String passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(0, request.Nickname, request.Email, Role.CHEF);

        return Ok(user);
    }
    
    [HttpPost("login")]
    public ActionResult<User> Login(UserRequest request)
    {

        if (_userRepository.FindUserByNickname(request.Nickname).Result != null)
        {
            return BadRequest("Nickname is already present in database");
        }

        var user = _userRepository.FindUserById(request.Id).Result;

        if (user == null) // if not present
        {
            return BadRequest("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password");
        }

        var token = CreateToken(user);
        
        return Ok(token);
    }

    private String CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Nickname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.GetStringValue())
        };
        
        var tokenValue = _configuration.GetSection("AppSettings").GetValue<string>("Token");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(30), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}