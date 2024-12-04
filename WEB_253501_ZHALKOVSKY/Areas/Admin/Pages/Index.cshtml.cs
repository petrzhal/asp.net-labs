using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Services.ProductService;

namespace web_253501_zhalkovsky.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public List<Dish> Products { get; set; } = new List<Dish>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            CurrentPage = pageNo;

            var response = await _productService.GetProductListAsync(null, pageNo);

            if (response.Data != null)
            {
                Products = response.Data?.Items ?? new List<Dish>();
                TotalPages = response.Data?.TotalPages ?? 0;

            }
        }
    }
}
