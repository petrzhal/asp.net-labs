using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_253501_zhalkovsky.API.Controllers;
using web_253501_zhalkovsky.API.Services;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using NSubstitute;
using Xunit;

namespace web_253501_zhalkovsky.Tests
{
    public class ProductControllerTests
    {
        private readonly ICategoryService _mockCategoryService;
        private readonly IProductService _mockProductService;
        private readonly DishesController _controller;

        public ProductControllerTests()
        {
            _mockCategoryService = Substitute.For<ICategoryService>();
            _mockProductService = Substitute.For<IProductService>();
            var mockLogger = Substitute.For<ILogger<DishesController>>();

            _controller = new DishesController(_mockProductService, mockLogger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task GetDishes_ReturnsNotFound_WhenCategoryListNotSuccessful()
        {
            // Arrange
            _mockCategoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Error("Failed to load categories"));

            // Act
            var result = await _controller.GetDishes("");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(null, notFoundResult.Value);
        }

        [Fact]
        public async Task GetDishes_ReturnsNotFound_WhenProductListNotSuccessful()
        {
            // Arrange
            _mockCategoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Success(new List<Category>()));
            _mockProductService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(ResponseData<ListModel<Dish>>.Error("Product fetch failed"));

            // Act
            var result = await _controller.GetDishes("");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Product fetch failed", notFoundResult.Value);
        }

        [Fact]
        public async Task GetDishes_ReturnsProductList_WhenSuccessful_WithoutCategory()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Drinks", NormalizedName = "drinks" },
                new Category { Id = 2, Name = "Snacks", NormalizedName = "snacks" }
            };
            var products = new ListModel<Dish>
            {
                Items = new List<Dish>
                {
                    new Dish { Id = 1, Name = "Coke" },
                    new Dish { Id = 2, Name = "Pepsi" }
                }
            };

            _mockCategoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Success(categories));
            _mockProductService.GetProductListAsync(null, Arg.Any<int>(), Arg.Any<int>())
                .Returns(ResponseData<ListModel<Dish>>.Success(products));

            // Act
            var result = await _controller.GetDishes(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnData = Assert.IsType<ResponseData<ListModel<Dish>>>(okResult.Value);
            Assert.Equal(products, returnData.Data);
        }

        [Fact]
        public async Task GetDishes_ReturnsProductList_WhenSuccessful_WithCategory()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Drinks", NormalizedName = "drinks" },
                new Category { Id = 2, Name = "Snacks", NormalizedName = "snacks" }
            };
            var products = new ListModel<Dish>
            {
                Items = new List<Dish>
                {
                    new Dish { Id = 1, Name = "Coke" },
                    new Dish { Id = 2, Name = "Pepsi" }
                }
            };

            var categoryName = "Drinks";
            _mockCategoryService.GetCategoryListAsync().Returns(ResponseData<List<Category>>.Success(categories));
            _mockProductService.GetProductListAsync(categoryName, Arg.Any<int>(), Arg.Any<int>())
                .Returns(ResponseData<ListModel<Dish>>.Success(products));

            // Act
            var result = await _controller.GetDishes(categoryName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnData = Assert.IsType<ResponseData<ListModel<Dish>>>(okResult.Value);
            Assert.Equal(products, returnData.Data);
            // Additional checks can be added here if necessary
        }
    }
}
