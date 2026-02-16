using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.Models;
namespace CS_APIServerProject.Data
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options) { 
        }
        //Adding Product in DataBase
        public DbSet<Product> Products => Set<Product>();

        //Adding Characteristics in DB
        public DbSet<Characteristics> Characteristics => Set<Characteristics>();

        //Adding User in DataBase
        public DbSet<User> Users => Set<User>();
        //Adding Order in DataBase
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.Characteristics);

            //Characteristics
            //modelBuilder.Entity<Characteristics>();

            //User
            modelBuilder.Entity<User>();

            //Order with Foreign Key for products with type of connection many to many.
            // Configure Order -> OrderItem as regular entity relationship (not owned)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
    
            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
