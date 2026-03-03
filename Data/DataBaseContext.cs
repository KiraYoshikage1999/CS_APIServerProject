using Auth.Data;
using CS_APIServerProject.Model;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CS_APIServerProject.Data
{
    public class DataBaseContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { 
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
        //Adding token system
        public DbSet<RefreshToken> RefreshTokens { get; set; }

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


            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                b.HasIndex(x => x.Token).IsUnique();

                b.HasOne(x => x.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
