using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_253501_zhalkovsky.Domain.Models
{
    public class ListModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 1;
    }
}
