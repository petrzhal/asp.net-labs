using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Services.ProductService;

namespace web_253501_zhalkovsky.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService; 

        public DetailsModel(IProductService productService)
        {
            _productService = productService; 
        }

        public Dish dish { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data == null)
            {
                Console.WriteLine("No data retrieved for the product ID: " + id); 
                return NotFound();
            }
            else
            {
                dish = response.Data;
                Console.WriteLine($"Product loaded: {dish.Name}, Image: {dish.Image}");
            }
            return Page();
        }

    }
}
