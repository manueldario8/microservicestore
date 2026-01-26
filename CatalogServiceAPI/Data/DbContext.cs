using CatalogServiceAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Data
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Providers
            modelBuilder.Entity<Provider>()
                .Property(p => p.Code);
            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Code)               
                .IsUnique();
                
            //Categories
            modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

            modelBuilder.Entity<Category>()
            .Property(c => c.Id);

            // Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(p => p.Products)
                .HasPrincipalKey(b => b.Code)
                .HasForeignKey(p => p.ProviderCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.ProviderCode, p.ProductCode })
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Id);
        }






    }
}
