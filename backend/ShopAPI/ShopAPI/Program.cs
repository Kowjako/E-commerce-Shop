using Infrastructure.Data.SeedData;
using ShopAPI.Extensions;
using ShopAPI.Middlewares;

#region DI

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.RegisterAppServices(builder.Configuration);
builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.ConfigureSwaggerImpl(); // from extensions

var app = builder.Build();

#endregion

#region HTTP - Pipeline

app.UseMiddleware<ExceptionMiddleware>(); // Middleware for errors
app.UseStatusCodePagesWithReExecute("/fallback/{0}"); // Catch unexisting endpoints and redirect to format response
app.UseSwaggerDocumentation(); // from extensions

// Our API will serve images inside wwwroot/images since our Products
// PictureUrl are referenced to them
app.UseStaticFiles();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed DB
using var scope = app.Services.CreateScope();
await StoreContextSeeder.SeedDataAsync(scope);

app.Run();

#endregion

