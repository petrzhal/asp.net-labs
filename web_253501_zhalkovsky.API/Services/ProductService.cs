using web_253501_zhalkovsky.API.Data;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace web_253501_zhalkovsky.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {

            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Dishes.AsQueryable();

            query = query.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));

            var count = await query.CountAsync();

            if (count == 0)
            {
                return ResponseData<ListModel<Dish>>.Error("Нет блюд в данной категории."); 
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return ResponseData<ListModel<Dish>>.Error("No such page");
            }

            var dataList = new ListModel<Dish>
            {
                Items = await query.OrderBy(d => d.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return ResponseData<ListModel<Dish>>.Success(dataList);
        }


        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var product = await _context.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
            if (product == null)
            {
                return ResponseData<Dish>.Error("Product not found");
            }
            return ResponseData<Dish>.Success(product);
        }

        public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product)
        {
            var existingProduct = await _context.Dishes.FindAsync(id);
            if (existingProduct == null)
            {
                return ResponseData<Dish>.Error("Product not found");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Calories = product.Calories;
            existingProduct.Image = product.Image;
            existingProduct.CategoryId = product.CategoryId;

            _context.Dishes.Update(existingProduct);
            await _context.SaveChangesAsync();

            return ResponseData<Dish>.Success(existingProduct);
        }

        public async Task<ResponseData<Dish>> DeleteProductAsync(int id)
        {
            var product = await _context.Dishes.FindAsync(id);
            if (product == null)
            {
                return ResponseData<Dish>.Error("Product not found");
            }

            _context.Dishes.Remove(product);
            await _context.SaveChangesAsync();

            return ResponseData<Dish>.Success(product);
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
        {
            _context.Dishes.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Dish>.Success(product);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var product = await _context.Dishes.FindAsync(id);
            if (product == null)
            {
                return ResponseData<string>.Error("Product not found");
            }

            var filePath = "/images/" + formFile.FileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.Image = filePath;
            _context.Dishes.Update(product);
            await _context.SaveChangesAsync();

            return ResponseData<string>.Success(filePath);
        }
    }
}
