using Microsoft.EntityFrameworkCore;

namespace State.Services
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<State.Models.State> States { get; set; }
    }
}
