using Microsoft.AspNetCore.Mvc;
using web_253501_zhalkovsky.API.Services;
using web_253501_zhalkovsky.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return Ok(result);
        }
    }
}
