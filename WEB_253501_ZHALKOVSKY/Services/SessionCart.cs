using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.Services
{
    public class SessionCart : Cart
    {
        private const string CartSessionKey = "cart";
        private ISession Session { get; }

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext!.Session;
            LoadFromSession();
        }

        private void LoadFromSession()
        {
            var sessionData = Session.GetString(CartSessionKey);
            if (sessionData != null)
            {
                CartItems = JsonConvert.DeserializeObject<Dictionary<int, CartItem>>(sessionData) ?? new();
            }
            Console.WriteLine("CartItems: " + CartItems);
        }

        private void SaveToSession()
        {
            Session.SetString(CartSessionKey, JsonConvert.SerializeObject(CartItems));
            Console.WriteLine("Saving CartItems: " + CartItems);
        }

        public override void AddToCart(Dish dish)
        {
            base.AddToCart(dish);
            SaveToSession();
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            SaveToSession();
        }

        public override void ClearAll()
        {
            base.ClearAll();
            SaveToSession();
        }
    }
}
