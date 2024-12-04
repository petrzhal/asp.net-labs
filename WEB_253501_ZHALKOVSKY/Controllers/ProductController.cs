using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_253501_zhalkovsky.Services.CategoryService;
using web_253501_zhalkovsky.Services.ProductService;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Extensions;

namespace web_253501_zhalkovsky.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }
        [Route("/Catalog/{category?}")]

        public async Task<IActionResult> Index(string? category = null, int pageNo = 1)
        {
            _logger.LogInformation("Fetching categories...");
            var response = await _categoryService.GetCategoryListAsync();

            if (response == null || response.Data == null)
            {
                _logger.LogWarning("Categories response is null.");
                return View(new ListModel<Dish>());
            }

            _logger.LogInformation($"Categories fetched: {response.Data.Count}");
            var categories = response.Data;

            var currentCategory = categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
            ViewData["categories"] = categories;
            ViewData["currentCategory"] = currentCategory;
            ViewData["currentCategoryNormalizedName"] = category;

            _logger.LogInformation("Fetching product list...");
            var products = await _productService.GetProductListAsync(category, pageNo);

            if (products == null || products.Data == null)
            {
                _logger.LogWarning("Products response is null.");
                var emptyModel = new ListModel<Dish>();

                if (Request.IsAjaxRequest())
                {
                    _logger.LogInformation("Returning partial view for empty product list.");
                    return PartialView("_ListPartial", emptyModel);
                }

                return View(emptyModel);
            }

            _logger.LogInformation($"Products fetched: {products.Data.Items.Count}");
            var model = products.Data;

            if (Request.IsAjaxRequest())
            {
                _logger.LogInformation("Returning partial view for products.");
                return PartialView("_ListPartial", model);
            }

            _logger.LogInformation("Returning full view for products.");
            return View(model);
        }
    }
}
