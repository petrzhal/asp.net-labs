using System.Text.Json.Serialization;

namespace web_253501_zhalkovsky.Models
{
    public class ItemsContainer<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
