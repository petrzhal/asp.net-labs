using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Services.CategoryService;
using web_253501_zhalkovsky.Services.ProductService;
namespace web_253501_zhalkovsky.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment; 

        [BindProperty]
        public IFormFile? UploadFile { get; set; } 
        public EditModel(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
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

            dish = response.Data;

            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(dish.Id);
                if (existingProduct.Data == null)
                {
                    return NotFound();
                }

                if (UploadFile != null)
                {
                    var uploadPath = Path.Combine(_environment.WebRootPath, "images");
                    var fileName = Path.GetFileName(UploadFile.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadFile.CopyToAsync(stream);
                    }

                    dish.Image = $"/images/{fileName}";
                }
                else
                {
                    dish.Image = existingProduct.Data.Image;
                }

                await _productService.UpdateProductAsync(dish.Id, dish, UploadFile);
            }
            catch (Exception)
            {
                if (!await DetailExists(dish.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> DetailExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Data != null;
        }
    }

}

