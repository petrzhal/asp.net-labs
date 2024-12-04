using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.API.Services
{
    public interface ICategoryService
    {
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
