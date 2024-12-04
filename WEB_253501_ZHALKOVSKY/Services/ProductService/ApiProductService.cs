using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.Models;
using web_253501_zhalkovsky.Services.FileService;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Services.Authentication;

namespace web_253501_zhalkovsky.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        private readonly IFileService _fileService;
        private readonly string _defaultPageSize;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiProductService(HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiProductService> logger,
        IFileService fileService,
        ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _defaultPageSize = configuration.GetSection("ItemsPerPage").Value ?? "3";
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}Dishes");

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"/{categoryNormalizedName}");
            }

            var queryParameters = new List<string>();
            if (pageNo > 1)
            {
                queryParameters.Add($"pageNo={pageNo}");
            }
            if (pageSize.ToString() != _defaultPageSize)
            {
                queryParameters.Add($"pageSize={pageSize}");
            }

            if (queryParameters.Count > 0)
            {
                urlString.Append("?" + string.Join("&", queryParameters));
            }

            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации: {ex.Message}");
                    return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка получения данных от сервера: {response.StatusCode}");
            return ResponseData<ListModel<Dish>>.Error($"Ошибка: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync($"dishes/{id}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации: {ex.Message}");
                    return ResponseData<Dish>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка получения данных продукта: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Ошибка: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            product.Image = "Images/noimage.jpg";

            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

            var uri = new Uri(_httpClient.BaseAddress, "Dishes");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Ошибка создания объекта: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            _logger.LogInformation($"Token: {await _tokenAccessor.GetAccessTokenAsync()}");

            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

            var response = await _httpClient.PutAsJsonAsync($"dishes/{id}", product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации при обновлении продукта: {ex.Message}");
                    return ResponseData<Dish>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка обновления продукта: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Ошибка: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> DeleteProductAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync($"dishes/{id}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации при удалении продукта: {ex.Message}");
                    return ResponseData<Dish>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка удаления продукта: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Ошибка: {response.StatusCode}");
        }
    }
}
