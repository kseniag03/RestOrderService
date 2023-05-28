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
    /// Регистратор сообщений и ошибок.
    /// </summary>
    private readonly ILogger<OrderController> _logger;

    /// <summary>
    /// Хранилище данных о заказах.
    /// </summary>
    private readonly IOrderRepository _orderRepository;
    
    public OrderController(IConfiguration configuration, IOrderRepository orderRepository, [FromServices]ILogger<OrderController> logger)
    {
        _configuration = configuration;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [HttpGet("get-order-by-id")]
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
}