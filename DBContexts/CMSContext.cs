using CustomerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.DBContexts
{
    public class CMSContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public CMSContext(DbContextOptions<CMSContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Product Configuration
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.ExternalId, p.ProductCatalogId })
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCatalog)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductCatalogId);

            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.Rating);

            // ProductCatalog Configuration
            modelBuilder.Entity<ProductCatalog>()
                .HasKey(pc => pc.ProductCatalogId);

            modelBuilder.Entity<ProductCatalog>()
                .HasMany(pc => pc.Products)
                .WithOne(p => p.ProductCatalog)
                .HasForeignKey(p => p.ProductCatalogId);

            // Order Configuration
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            // OrderItem Configuration
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}