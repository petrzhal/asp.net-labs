//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using web_253501_zhalkovsky.Domain.Entities;
//using web_253501_zhalkovsky.Domain.Models;

//namespace web_253501_zhalkovsky.Services.ProductService
//{
//    public class MemoryProductService : IProductService
//    {
//        private readonly List<Dish> _dishes;

//        public MemoryProductService()
//        {
//            _dishes = new List<Dish>();
//        }

//        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
//        {
//            var filteredDishes = _dishes; // Фильтрация по категории, если нужно
//            var totalDishes = filteredDishes.Count;

//            var pagedDishes = filteredDishes.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//            var result = new ResponseData<ListModel<Dish>>
//            {
//                Data = new ListModel<Dish>
//                {
//                    Items = pagedDishes,
//                    // Убедитесь, что TotalCount добавлено в ListModel<Dish>
//                },
//                Successful = true // Предполагается, что это правильное имя свойства
//            };

//            return Task.FromResult(result);
//        }

//        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
//        {
//            var dish = _dishes.FirstOrDefault(d => d.Id == id);
//            if (dish != null)
//            {
//                return Task.FromResult(ResponseData<Dish>.Success(dish));
//            }
//            return Task.FromResult(ResponseData<Dish>.Error($"Dish with id {id} not found."));
//        }

//        public Task<ResponseData<Dish>> CreateProductAsync(Dish product)
//        {
//            _dishes.Add(product);
//            return Task.FromResult(ResponseData<Dish>.Success(product));
//        }

//        public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product)
//        {
//            var existingDish = _dishes.FirstOrDefault(d => d.Id == id);
//            if (existingDish != null)
//            {
//                existingDish.Name = product.Name;
//                existingDish.Description = product.Description;
//                return Task.FromResult(ResponseData<Dish>.Success(existingDish));
//            }
//            return Task.FromResult(ResponseData<Dish>.Error($"Dish with id {id} not found."));
//        }

//        public Task<ResponseData<Dish>> DeleteProductAsync(int id)
//        {
//            var existingDish = _dishes.FirstOrDefault(d => d.Id == id);
//            if (existingDish != null)
//            {
//                _dishes.Remove(existingDish);
//                return Task.FromResult(ResponseData<Dish>.Success(existingDish));
//            }
//            return Task.FromResult(ResponseData<Dish>.Error($"Dish with id {id} not found."));
//        }
//    }
//}
