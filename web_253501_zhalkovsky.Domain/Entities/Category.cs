using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace web_253501_zhalkovsky.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
