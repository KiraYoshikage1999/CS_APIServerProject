using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.Models;
namespace CS_APIServerProject.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DbContext> options) : base(options) { 
        }
        //Adding Product in DataBase
        public DbSet<Product> Products => Set<Product>();
        //Adding User in DataBase
        public DbSet<User> Users => Set<User>();
        //Adding Order in DataBase
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.Characteristics);

            //User
            modelBuilder.Entity<User>();

            //Order with Foreign Key for products with type of connection many to many.
            modelBuilder.Entity<Order>()
                .OwnsMany(p => p.FK_Products);


            base.OnModelCreating(modelBuilder);
        }
    }
}
