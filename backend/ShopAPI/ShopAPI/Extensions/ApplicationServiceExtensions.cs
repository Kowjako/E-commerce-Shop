using Core.Interface;
using Infrastructure.Data.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Errors;

namespace ShopAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<StoreContext>(p =>
            {
                p.UseSqlite(config.GetConnectionString("Default"));
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
                opt.AddPolicy("CORS", builder =>
                {
                    builder.WithOrigins("https://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
