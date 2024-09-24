using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace InventoryManagementAPI
{
    public class Program
    {
        public static async Task Main(string[] args) // Gör metoden asynkron
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<InventoryManagementAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagementAPIContext") ?? throw new InvalidOperationException("Connection string 'InventoryManagementAPIContext' not found.")));

            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementAPIContext");
            Console.WriteLine("Connectionstring: " + connectionString);
            builder.Services.AddTransient<Data.ProductManager>();
            builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true; // För läsbarhet
            });

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<InventoryManagementAPIContext>().AddDefaultTokenProviders();

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

            // Anropa SeedTestDataAsync för att skapa testdata
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<InventoryManagementAPIContext>();
                await context.SeedTestDataAsync();
            }

            app.Run();
        }
    }
}
