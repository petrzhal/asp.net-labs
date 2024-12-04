using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace web_253501_zhalkovsky.API.Services
{
    public interface IProductService
    {
        Task<ResponseData<ListModel<Dish>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3);

        Task<ResponseData<Dish>> GetProductByIdAsync(int id);
        Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product);
        Task<ResponseData<Dish>> DeleteProductAsync(int id);
        Task<ResponseData<Dish>> CreateProductAsync(Dish product);
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
