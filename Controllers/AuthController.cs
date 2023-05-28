using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestOrderService.Libraries;
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
    public async Task<ActionResult<User>> Register(UserRequest request)
    {
        if (await _userRepository.FindUserByNickname(request.Nickname) != null)
        {
            return BadRequest("Nickname is already present in database");
        }
        if (await _userRepository.FindUserByLogin(request.Login) != null)
        {
            return BadRequest("Email is already present in database");
        }
        if (!_userRepository.IsLoginValid(request.Login))
        {
            return BadRequest("Email is not valid");
        }
        if (!_userRepository.IsPasswordSafe(request.Password))
        {
            return BadRequest("Password is not safe");
        }

        var id = (await _userRepository.FindAllUsers()).Count;
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(id, request.Nickname, request.Login, passwordHash, Role.Customer);
        
        await _userRepository.AddNewUser(user);

        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserRequest request)
    {
        var user = await _userRepository.FindUserByLogin(request.Login);
        if (user == null)
        {
            return NotFound("User not found");
        }
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password");
        }

        var token = CreateToken(user);
        user.Token = token;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateUser(user);
        
        return Ok(token);
    }
    
    [HttpGet("get-all-users")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var list = await _userRepository.FindAllUsers();
        if (list.Count == 0)
        {
            return NotFound("Users list is empty");
        }
        return Ok(list);
    }

    private String CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Nickname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.GetStringValue())
        };

        var tokenValue = _configuration.GetSection("AppSettings:Token").Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue ?? "ab1cd2ef3gh4ij5kl"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(30), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}