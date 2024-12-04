using Microsoft.EntityFrameworkCore;
using web_253501_zhalkovsky.Domain.Entities;

namespace web_253501_zhalkovsky.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
