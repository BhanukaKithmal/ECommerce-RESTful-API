using ECommerceRESTfulAPI.Repositories.Implementations;
using ECommerceRESTfulAPI.Repositories.Interfaces;
using ECommerceRESTfulAPI.Services.Implementations;
using ECommerceRESTfulAPI.Services.Interfaces;
using EntityFrameworkCore.MySQL.Data;
using EntityFrameworkCore.MySQL.Repositories.Interfaces;
using EntityFrameworkCore.MySQL.Repositories;
using EntityFrameworkCore.MySQL.Services.Interfaces;
using EntityFrameworkCore.MySQL.Services;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Register Repositories
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // Register Services
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        // Register DbContext with MySQL
        var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Configure JSON Serialization to ignore reference cycles and prevent $id
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        // Swagger/OpenAPI configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
