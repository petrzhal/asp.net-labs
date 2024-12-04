using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Domain.Entities;

namespace web_253501_zhalkovsky.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        public virtual void AddToCart(Dish dish)
        {
            if (CartItems.ContainsKey(dish.Id))
            {
                CartItems[dish.Id].Count++;
            }
            else
            {
                CartItems[dish.Id] = new CartItem { Item = dish, Count = 1 };
            }
        }

        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count => CartItems.Sum(item => item.Value.Count);

        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public double TotalCalories => (double)CartItems.Sum(item => item.Value.Item.Calories * item.Value.Count);
    }
}
