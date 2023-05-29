using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestOrderService.Models;
using RestOrderService.Models.Enums;
using RestOrderService.Models.Requests;
using RestOrderService.Repositories;

namespace RestOrderService.Controllers;

/// <summary>
/// Class-controller for processing user's queries.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{
    /// <summary>
    /// App settings (for getting key for token generation).
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Storage of users data.
    /// </summary>
    private readonly IUserRepository _userRepository;
    
    public AuthController(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    [HttpPost("add-manager-for-testing")]
    public async Task<ActionResult<User>> AddManager(UserRequest request)
    {
        try
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User(request.Nickname, request.Login, passwordHash, Role.Manager);
            await _userRepository.AddNewUser(user);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("add-chef-for-testing")]
    public async Task<ActionResult<User>> AddChef(UserRequest request)
    {
        try
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User(request.Nickname, request.Login, passwordHash, Role.Chef);
            await _userRepository.AddNewUser(user);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRequest request)
    {
        try
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
            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User(request.Nickname, request.Login, passwordHash, Role.Customer);
        
            await _userRepository.AddNewUser(user);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserRequest request)
    {
        try
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

            await _userRepository.UpdateUser(user);
        
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("get-user-by-token")]
    public async Task<ActionResult<User>> GetUserByToken(string token)
    {
        try
        {
            var user = await _userRepository.FindUserByToken(token);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("get-all-users")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        try
        {
            var list = await _userRepository.FindAllUsers();
            if (list.Count == 0)
            {
                return NotFound("Users list is empty");
            }
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("change-user-role")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<User>> ChangeRole(int id, string roleStr)
    {
        try
        {
            var user = await _userRepository.FindUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.Role == Role.Manager)
            {
                return BadRequest("You cannot change manager's role");
            }
            var role = Role.Customer;
            switch (roleStr)
            {
                case "customer":
                    break;
                case "chef":
                    role = Role.Chef;
                    break;
                case "manager":
                    role = Role.Manager;
                    break;
                default:
                    return BadRequest("Not supported role");
            }
        
            user.Role = role;
            await _userRepository.UpdateUser(user);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Creates unique token for current user's session.
    /// </summary>
    /// <param name="user"> Current user </param>
    /// <returns> String representation of token </returns>
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Nickname),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        var tokenValue = _configuration["AppSettings:Token"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}