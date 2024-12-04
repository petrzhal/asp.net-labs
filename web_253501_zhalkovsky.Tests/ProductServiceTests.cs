using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using web_253501_zhalkovsky.Domain.Entities;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.API.Data;
using web_253501_zhalkovsky.API.Services;
using Xunit;

namespace web_253501_zhalkovsky.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            context.Dishes.AddRange(
                new Dish { Id = 1, Name = "Product1", CategoryId = 1 },
                new Dish { Id = 2, Name = "Product2", CategoryId = 1 },
                new Dish { Id = 3, Name = "Product3", CategoryId = 1 },
                new Dish { Id = 4, Name = "Product4", CategoryId = 2 },
                new Dish { Id = 5, Name = "Product5", CategoryId = 2 }
            );
            context.Categories.AddRange(
                new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Category { Id = 2, Name = "Category2", NormalizedName = "category2" }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context); 

            var result = await service.GetProductListAsync(null);

            Assert.IsType<ResponseData<ListModel<Dish>>>(result);
            Assert.True(result.Successful);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages); 
            Assert.Equal("Product1", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceReturnsSecondPageOfItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync(null, pageNo: 2);

            Assert.True(result.Successful);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(2, result.Data.Items.Count); 
            Assert.Equal("Product4", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceFiltersItemsByCategory()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync("category1");

            Assert.True(result.Successful);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.All(result.Data.Items, p => Assert.Equal(1, p.CategoryId));
        }

        [Fact]
        public async Task ServiceRestrictsPageSizeToMaximum()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync(null, pageSize: 100);

            Assert.True(result.Successful);
            Assert.Equal(result.Data.TotalPages, result.Data.Items.Count / 3);
            Assert.Equal(5, result.Data.Items.Count);
        }

        [Fact]
        public async Task ServiceReturnsErrorWhenPageNumberExceedsTotalPages()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync(null, pageNo: 10);

            Assert.False(result.Successful);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
