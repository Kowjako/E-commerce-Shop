using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters()
                        {
                            // validate token issuer and issuer key
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                            ValidIssuer = config["Token:Issuer"],
                            // since we doesnt add audience to our token during
                            // generation inside (TokenService) - skip it,
                            // but we add issuer to token, so in DI configuration we should
                            // validate jwt issuer
                            ValidateAudience = false
                        };
                    });

            services.AddAuthorization();

            return services;
        }
    }
}
