using Core.Entities;
using System.Text.Json;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeeder
    {
        public static async Task SeedDataAsync(StoreContext context)
        {
            if (!context.ProductBrand.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrand.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                var productTypeData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var productType = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                context.ProductTypes.AddRange(productType);
            }

            if (!context.Products.Any())
            {
                var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var product = JsonSerializer.Deserialize<List<Product>>(productData);
                context.Products.AddRange(product);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
