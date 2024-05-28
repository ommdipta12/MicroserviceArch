using Microsoft.EntityFrameworkCore;

namespace City.Services
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<City.Models.City> Cites { get; set; }
    }
}
