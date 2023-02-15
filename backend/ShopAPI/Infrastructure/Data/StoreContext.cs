using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> context)
            : base(context) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pick config from EntityTypeBuilder
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Since SQLite doesnt support sorting by decimal
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                modelBuilder.Entity<Product>().Property(x => x.Price).HasConversion<double>();
                modelBuilder.Entity<Order>().Property(x => x.SubTotal).HasConversion<double>();
                modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasConversion<double>();
                modelBuilder.Entity<DeliveryMethod>().Property(x => x.Price).HasConversion<double>();
            }
        }
    }
}
