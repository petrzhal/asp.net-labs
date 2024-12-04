using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;

namespace web_253501_zhalkovsky.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id = 1, Name = "Стартеры", NormalizedName = "starters"},
                new Category {Id = 2, Name = "Салаты", NormalizedName = "salads"},
                new Category {Id = 3, Name = "Супы", NormalizedName = "soups"},
                new Category {Id = 4, Name = "Основные блюда", NormalizedName = "main-dishes"},
                new Category {Id = 5, Name = "Напитки", NormalizedName = "drinks"},
                new Category {Id = 6, Name = "Десерты", NormalizedName = "desserts"},
                new Category {Id = 7, Name = "Пельмени", NormalizedName = "dumplings"},
                new Category {Id = 8, Name = "Мясо", NormalizedName = "meat"},
                new Category {Id = 9, Name = "Рис", NormalizedName = "rice"},
                new Category {Id = 10, Name = "Паста", NormalizedName = "pasta"},
                new Category {Id = 11, Name = "Тако", NormalizedName = "tacos"},
                new Category {Id = 12, Name = "Блины", NormalizedName = "pancakes"}
            };

            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }

}
