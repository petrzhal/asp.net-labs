using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Services.CategoryService;
using web_253501_zhalkovsky.Services.ProductService;

namespace web_253501_zhalkovsky.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment; 

        public CreateModel(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Dish dish { get; set; }

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public async Task OnGetAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();

            if (response?.Data != null)
            {
                ViewData["CategoryId"] = new SelectList(response.Data, "Id", "Name");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(new List<Category>(), "Id", "Name");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
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

            await _productService.CreateProductAsync(dish, UploadFile);

            return RedirectToPage("./Index");
        }
    }
}