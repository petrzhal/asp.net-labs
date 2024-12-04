using Microsoft.AspNetCore.Mvc;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            var cartTotal = 0.0;
            var itemCount = 0;

            ViewBag.CartTotal = cartTotal;
            ViewBag.ItemCount = itemCount;

            return View(_cart);
        }
    }
}
