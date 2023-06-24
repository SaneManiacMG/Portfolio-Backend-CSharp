using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Csharp.Models.Entities;

namespace Portfolio.Backend.Csharp.Configs
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(e => e.Role)
                .HasConversion<string>();
            modelBuilder.Entity<Login>().Property(e => e.AccountStatus)
                .HasConversion<string>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> LoginDetails { get; set; }
    }
}
