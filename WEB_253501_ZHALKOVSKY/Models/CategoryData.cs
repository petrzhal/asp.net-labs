using System.Text.Json.Serialization;
using web_253501_zhalkovsky.Domain.Entities;

namespace web_253501_zhalkovsky.Models
{
    public class CategoryData
    {
        [JsonPropertyName("$values")] 
        public List<Category> Values { get; set; }
    }

}
