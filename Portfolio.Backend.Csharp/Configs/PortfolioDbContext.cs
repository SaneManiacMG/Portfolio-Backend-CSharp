using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Csharp.Models.Entities;

namespace Portfolio.Backend.Csharp.Configs
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> LoginDetails { get; set; }
    }
}
