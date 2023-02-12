using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentityCore<AppUser>(opt =>
            {

            })
            .AddEntityFrameworkStores<AppIdentityDbContext>();
            // .AddSignInManager<SignInManager, AppUser>();

            services.AddAuthentication();
            services.AddAuthorization();

            return services;
        }
    }
}
