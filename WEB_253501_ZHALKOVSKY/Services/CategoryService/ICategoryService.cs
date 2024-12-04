using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }

}
