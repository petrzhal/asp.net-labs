using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.Services.ProductService;

namespace web_253501_zhalkovsky.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;

        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        [HttpPost("Add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (result.Successful)
            {
                _cart.AddToCart(result.Data);
            }

            return Redirect(returnUrl ?? "/");
        }

        [HttpGet("View")]
        public IActionResult ViewCart()
        {
            return View(_cart);
        }

        [HttpPost("Remove/{id:int}")]
        public IActionResult Remove(int id)
        {
            _cart.RemoveItems(id); 
            return RedirectToAction("ViewCart");
        }

        [HttpPost("Clear")]
        public IActionResult Clear()
        {
            _cart.ClearAll(); 
            return RedirectToAction("ViewCart");
        }
    }
}
