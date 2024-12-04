using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.Models;

namespace web_253501_zhalkovsky.Services.ProductService
{
    public interface IProductService
    {
        Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        Task<ResponseData<Dish>> GetProductByIdAsync(int id);
        Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
        Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile);
        Task<ResponseData<Dish>> DeleteProductAsync(int id);
    }
}
