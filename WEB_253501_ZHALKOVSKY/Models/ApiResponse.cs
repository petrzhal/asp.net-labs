using System.Text.Json.Serialization;

namespace web_253501_zhalkovsky.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; }

        public bool Successful { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
