using Core.Entities;
using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeeder
    {
        private static async Task SeedDataAsyncInternal(StoreContext context)
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

        public static async Task SeedIdentityAsyncInternal(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address()
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "10 The Street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "90210"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        public static async Task SeedDataAsync(IServiceScope scope)
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var logger = services.GetRequiredService<ILogger<StoreContextSeeder>>();
            try
            {
                await context.Database.MigrateAsync();
                await SeedDataAsyncInternal(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during migration");
            }

            var contextIdentity = services.GetRequiredService<AppIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            try
            {
                await contextIdentity.Database.MigrateAsync();
                await SeedIdentityAsyncInternal(userManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during identity migraiton");
            }
        }
    }
}
