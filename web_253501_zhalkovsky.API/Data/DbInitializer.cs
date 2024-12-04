using Microsoft.EntityFrameworkCore;
using web_253501_zhalkovsky.Domain.Entities; 

namespace web_253501_zhalkovsky.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            await context.Database.MigrateAsync();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new() { Name = "Стартеры", NormalizedName = "starters" },
                    new() { Name = "Салаты", NormalizedName = "salads" },
                    new() { Name = "Супы", NormalizedName = "soups" },
                    new() { Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new() { Name = "Напитки", NormalizedName = "drinks" },
                    new() { Name = "Десерты", NormalizedName = "desserts" },
                    new() { Name = "Пельмени", NormalizedName = "dumplings" },
                    new() { Name = "Мясо", NormalizedName = "meat" },
                    new() { Name = "Рис", NormalizedName = "rice" },
                    new() { Name = "Паста", NormalizedName = "pasta" },
                    new() { Name = "Тако", NormalizedName = "tacos" },
                    new() { Name = "Блины", NormalizedName = "pancakes" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
            var BaseUrl = app.Configuration["AppUrl"];

            if (!context.Dishes.Any())
            {
                var dishes = new List<Dish>
                {
                    new() { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"{BaseUrl}Images/Суп-харчо.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "soups") },
                    new() { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"{BaseUrl}Images/Борщ.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "soups") },
                    new() { Name = "Пельмени", Description = "Пельмени с мясом", Calories = 500, Image = $"{BaseUrl}Images/Пельмени.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "dumplings") },
                    new() { Name = "Овощной салат", Description = "Свежий салат с овощами", Calories = 150, Image = $"{BaseUrl}Images/Овощной салат.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "salads") },
                    new() { Name = "Стейк", Description = "Сочный стейк с гарниром", Calories = 700, Image = $"{BaseUrl}Images/Стейк.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "meat") },
                    new() { Name = "Ризотто", Description = "Ризотто с грибами", Calories = 400, Image = $"{BaseUrl}Images/Ризотто.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "rice") },
                    new() { Name = "Спагетти Карбонара", Description = "Классическое итальянское блюдо", Calories = 600, Image = $"{BaseUrl}Images/Карбонара.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "pasta") },
                    new() { Name = "Курица гриль", Description = "Курица, запеченная на гриле", Calories = 350, Image = $"{BaseUrl}Images/Курица.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "meat") },
                    new() { Name = "Тако", Description = "Тако с курицей и овощами", Calories = 250, Image = $"{BaseUrl}Images/Тако.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "tacos") },
                    new() { Name = "Блины", Description = "Тонкие блины с начинкой", Calories = 200, Image = $"{BaseUrl}Images/Блины.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "pancakes") },
                    new() { Name = "Мясной рулет", Description = "Рулет из говядины с грибной начинкой", Calories = 650, Image = $"{BaseUrl}Images/Мясной рулет.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "meat") },
                    new() { Name = "Свинина на гриле", Description = "Сочные куски свинины, приготовленные на гриле", Calories = 750, Image = $"{BaseUrl}Images/Свинина.jpg", Category = context.Categories.FirstOrDefault(c => c.NormalizedName == "meat") }
                };

                context.Dishes.AddRange(dishes);
                await context.SaveChangesAsync();
            }
        }
    }
}
