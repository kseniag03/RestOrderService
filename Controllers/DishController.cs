using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestOrderService.Models;
using RestOrderService.Repositories;

namespace RestOrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class DishController: ControllerBase
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Хранилище данных о блюдах.
    /// </summary>
    private readonly IDishRepository _dishRepository;
    
    public DishController(IConfiguration configuration, IDishRepository dishRepository)
    {
        _configuration = configuration;
        _dishRepository = dishRepository;
    }

    [HttpGet("get-dish-by-id")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<Dish>> GetDishById(int id)
    {
        try
        {
            var dish = await _dishRepository.FindDishById(id);
            if (dish == null)
            {
                return NotFound("Dish not found");
            }
            return Ok(dish);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-all-dishes")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<List<Dish>>> GetAllDishes()
    {
        try
        {
            var dishes = await _dishRepository.FindAllDishes();
            if (dishes.Count == 0)
            {
                return NotFound("Dishes list is empty");
            }
            return Ok(dishes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-dish")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<string>> CreateDish(Dish dish)
    {
        try
        {
            if (dish.Id < 0 || dish.Quantity < 0 || dish.Price < 0)
            {
                return BadRequest("Invalid dish parameters");
            }
            var sameDish = await _dishRepository.FindDishById(dish.Id);
            if (sameDish != null)
            {
                if (sameDish.Name != dish.Name || sameDish.Description != dish.Description || sameDish.Price != dish.Price)
                {
                    return BadRequest("You are trying to add dish with different parameters but the same id that already exists in database");
                }
                return await UpdateDish(dish.Id, dish.Quantity + sameDish.Quantity);
            }
            await _dishRepository.AddNewDish(dish);
            return Ok($"Dish with id = {dish.Id} has been added to list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update-dish-quantity")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<string>> UpdateDish(int id, int quantity)
    {
        try
        {
            if (id < 0 || quantity < 0)
            {
                return BadRequest("Invalid input parameters");
            }
        
            var dish = await _dishRepository.FindDishById(id);
            if (dish == null)
            {
                return NotFound("Dish not found");
            }

            dish.Quantity = quantity;
            await _dishRepository.UpdateDish(dish);
        
            return Ok($"Quantity of dish with id = {id} has been changed to {dish.Quantity}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("reduce-dish-quantity")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<string>> DeleteDish(int id, int quantity)
    {
        try
        {
            if (id < 0 || quantity < 0)
            {
                return BadRequest("Invalid input parameters");
            }
        
            var dish = await _dishRepository.FindDishById(id);
            if (dish == null)
            {
                return NotFound("Dish not found");
            }

            switch (dish.Quantity - quantity)
            {
                case < 0:
                    return BadRequest("Not enough quantity, you cannot delete");
                case 0:
                    await _dishRepository.DeleteDish(dish);
                    return Ok($"Dish with id = {id} has been removed from list");
                default:
                    return await UpdateDish(id, dish.Quantity - quantity);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}