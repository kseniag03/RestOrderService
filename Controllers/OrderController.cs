using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Хранилище данных о заказах.
    /// </summary>
    private readonly IOrderRepository _orderRepository;
    
    public OrderController(IConfiguration configuration, IOrderRepository orderRepository)
    {
        _configuration = configuration;
        _orderRepository = orderRepository;
    }

    [HttpGet("get-order-by-id")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        try
        {
            var order = await _orderRepository.FindOrderById(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-all-orders")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        try
        {
            var list = await _orderRepository.FindAllOrders();
            if (list.Count == 0)
            {
                return NotFound("Orders list is empty");
            }
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("create-new-order")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<Order>> CreateOrder(int userId, DishesRequest dishRequests, string specialRequest)
    {
        try
        {
            foreach (var request in dishRequests.dishesList)
            {
                var dish = await _orderRepository.FindDishById(request.DishId);
                if (dish == null)
                {
                    return NotFound($"No such dish id = {request.DishId} in dish list");
                }
            }
            
            var order = new Order(userId, specialRequest);

            await _orderRepository.AddNewOrder(order, dishRequests.dishesList);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /*
    [HttpGet("get-user-by-cur-token")]
    public async Task<ActionResult<User>> GetUserByCurrentToken()
    {
        try
        {
            var user = await GetUserFromTokenByEmail();
        
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private async Task<ActionResult<User>> GetUserFromTokenByEmail()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token not found in the request header");
        }

        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        
        // Extract user information from claims
        var userEmail = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
        {
            return NotFound("Could not get email from token");
        }

        var user = await _orderRepository.FindUserByLogin(userEmail);
        if (user == null)
        {
            return NotFound("Could not get user by email");
        }

        return user;
    }
    */
}