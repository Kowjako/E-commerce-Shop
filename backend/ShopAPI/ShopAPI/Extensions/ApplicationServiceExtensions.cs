using Core.Interface;
using Infrastructure.Data.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Errors;
using StackExchange.Redis;
using Infrastructure.Services;

namespace ShopAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<StoreContext>(p =>
            {
                p.UseSqlite(config.GetConnectionString("Default"));
            });

            // Setting up Redis
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // It should be always after AddControllers()!!
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Any())
                                                         .SelectMany(x => x.Value.Errors)
                                                         .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins("https://localhost:4200");
                });
            });

            return services;
        }
    }
}
