using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Domain.Entities;

namespace web_253501_zhalkovsky.Domain.Models
{
    public class CartItem
    {
        public Dish Item { get; set; }
        public int Count { get; set; }
    }
}
