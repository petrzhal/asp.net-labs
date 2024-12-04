using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_253501_zhalkovsky.API.Services;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace web_253501_zhalkovsky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<DishesController> _logger;

        public DishesController(IProductService productService, ILogger<DishesController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{category?}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Dish>>>> GetDishes(string? category, int pageNo = 1, [FromQuery] int pageSize = 3)
        {
            var parameters = new
            {
                Category = category,
                PageNo = pageNo,
                PageSize = pageSize
            };

            _logger.LogInformation($"Received parameters: {JsonSerializer.Serialize(parameters)}");

            var result = await _productService.GetProductListAsync(category, pageNo, pageSize);
            if (result == null || !result.Successful)
            {
                return NotFound(result?.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDishById(int id)
        {
            _logger.LogInformation($"Received request to get dish with ID {id}");

            var result = await _productService.GetProductByIdAsync(id);
            if (result == null || !result.Successful)
            {
                return NotFound($"Dish with ID {id} not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Dish>>> AddDish([FromBody] Dish newDish)
        {
            if (newDish == null)
            {
                return BadRequest("Dish data is required.");
            }

            _logger.LogInformation($"Received request to add new dish: {JsonSerializer.Serialize(newDish)}");

            var result = await _productService.CreateProductAsync(newDish);
            if (result == null || !result.Successful)
            {
                return StatusCode(500, result?.ErrorMessage ?? "An error occurred while adding the dish.");
            }

            return CreatedAtAction(nameof(GetDishById), new { id = result.Data.Id }, result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult> DeleteDish(int id)
        {
            _logger.LogInformation($"Received request to delete dish with ID {id}");

            var result = await _productService.DeleteProductAsync(id);

            if (result == null || !result.Successful)
            {
                return NotFound($"Dish with ID {id} not found or could not be deleted.");
            }

            return NoContent(); 
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Dish>>> UpdateDish(int id, [FromBody] Dish updatedDish)
        {
            if (updatedDish == null)
            {
                return BadRequest("Dish data is required.");
            }

            _logger.LogInformation($"Received request to update dish with ID {id}");

            var result = await _productService.UpdateProductAsync(id, updatedDish);
            if (result == null || !result.Successful)
            {
                return NotFound($"Dish with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<string>>> SaveImage(int id, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var result = await _productService.SaveImageAsync(id, formFile);
            if (result == null || !result.Successful)
            {
                return NotFound($"Dish with ID {id} not found.");
            }

            return Ok(result);
        }
    }
}
