using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Services.CategoryService;
using web_253501_zhalkovsky.Services.ProductService;

namespace web_253501_zhalkovsky.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService; 
        private readonly ICategoryService _categoryService;

        public DeleteModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
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
                return NotFound();
            }
            else
            {
                dish = response.Data; 
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data != null)
            {
                await _productService.DeleteProductAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
