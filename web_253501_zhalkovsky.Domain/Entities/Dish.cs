using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_253501_zhalkovsky.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string? Description { get; set; } 
        public decimal Price { get; set; } 
        public decimal Calories { get; set; } 
        public string? Image { get; set; } 

        public int? CategoryId { get; set; } 
        public Category? Category { get; set; }
    }
}
