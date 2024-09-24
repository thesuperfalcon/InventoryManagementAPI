using InventoryManagementAPI.DAL;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                /*options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;*/ // Do not use reference handling
                options.JsonSerializerOptions.WriteIndented = true; // Optional: for readability
                //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            //builder.Services.AddControllers()
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //});            
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<InventoryManagementAPIContext>().AddDefaultTokenProviders();


            builder.Services.AddScoped<StorageManager>();
            builder.Services.AddScoped<InventoryTrackerManager>();

            //builder.Services.AddEntityFrameworkStores<InventoryManagementAPIContext>();

            //builder.Services.AddDefaultTokenProviders();
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
