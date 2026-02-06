using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.Models;
namespace CS_APIServerProject.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DbContext> options) : base(options) { 
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.Characteristics);
            base.OnModelCreating(modelBuilder);
        }
    }
}
