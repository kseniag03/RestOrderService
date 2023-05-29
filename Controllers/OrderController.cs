using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestOrderService.Models;
using RestOrderService.Models.Requests;
using RestOrderService.Repositories;

namespace RestOrderService.Controllers;

/// <summary>
/// Class-controller for processing authorised user's queries about orders.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    /// <summary>
    /// Storage of orders data.
    /// </summary>
    private readonly IOrderRepository _orderRepository;
    
    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet("get-order-by-id")]
    [Authorize(Roles = "Customer,Chef,Manager")]
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
    [Authorize(Roles = "Customer,Chef,Manager")]
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
    [Authorize(Roles = "Customer,Chef,Manager")]
    public async Task<ActionResult<Order>> CreateOrder(int userId, DishesRequest dishRequests, string specialRequest)
    {
        try
        {
            var dishesList = new List<OrderDish>();
            foreach (var request in dishRequests.DishesIdList)
            {
                var dish = await _orderRepository.FindDishById(request);
                if (dish == null)
                {
                    return NotFound($"No such dish id = {request} in dish list");
                }

                var orderDish = new OrderDish(dish.Id, dish.Quantity, dish.Price);
                dishesList.Add(orderDish);
            }
            
            var order = new Order(userId, specialRequest);

            await _orderRepository.AddNewOrder(order, dishesList);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}