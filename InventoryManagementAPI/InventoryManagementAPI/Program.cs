using InventoryManagementAPI.DAL;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementAPI
{
    public class Program
    {
        public static async Task Main(string[] args) // G�r metoden asynkron
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<InventoryManagementAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagementAPIContext") ?? throw new InvalidOperationException("Connection string 'InventoryManagementAPIContext' not found.")));

            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementAPIContext");
            Console.WriteLine("Connectionstring: " + connectionString);
            builder.Services.AddDbContext<Data.InventoryManagementAPIContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true; // F�r l�sbarhet
            });

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<InventoryManagementAPIContext>()
            .AddDefaultTokenProviders();

            
            builder.Services.AddScoped<StorageManager>();
            builder.Services.AddScoped<InventoryTrackerManager>();
            builder.Services.AddScoped<ProductManager>();
            builder.Services.AddScoped<UserManager>();
            //builder.Services.AddScoped<UserManager<User>();
            //builder.Services.AddScoped<RoleManager<Role>();

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
            // Så att huvudprogrammet kan hämta filer från API projektet.
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Anropa SeedTestDataAsync f�r att skapa testdata
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<InventoryManagementAPI.Models.Role>>();
                var userManager = services.GetRequiredService<UserManager<InventoryManagementAPI.Models.User>>();
               
                var context = scope.ServiceProvider.GetRequiredService<InventoryManagementAPIContext>();
                await context.CreateDefaultSlot();
                await context.SeedRolesAndAdminUser(roleManager, userManager);
                await context.SeedDevelopers();
                
                //await Data.InventoryManagementAPIContext.SeedRolesAndAdminUser(roleManager, userManager);
            }

            app.Run();
        }

    }
}
