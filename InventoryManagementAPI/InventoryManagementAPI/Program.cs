
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InventoryManagementAPI.Data;

namespace InventoryManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<InventoryManagementAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagementAPIContext") ?? throw new InvalidOperationException("Connection string 'InventoryManagementAPIContext' not found.")));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementAPIContext");
            Console.WriteLine("Connectionstring: " + connectionString);
            builder.Services.AddDbContext<Data.InventoryManagementAPIContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddTransient<Data.ProductManager>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
}
