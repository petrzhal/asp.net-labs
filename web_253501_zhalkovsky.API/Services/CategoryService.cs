using web_253501_zhalkovsky.API.Data;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace web_253501_zhalkovsky.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
