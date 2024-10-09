using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Identity;
using InventoryManagementAPI.Controllers;
using System.Data;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementAPI.Data
{
    public class InventoryManagementAPIContext : IdentityDbContext<User, Models.Role, string>
    {


        public InventoryManagementAPIContext (DbContextOptions<InventoryManagementAPIContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryManagementAPI.Models.Product> Products { get; set; }
        public DbSet<InventoryManagementAPI.Models.Storage> Storages { get; set; }
        public DbSet<InventoryManagementAPI.Models.Statistic> Statistics { get; set; } 
        public DbSet<InventoryManagementAPI.Models.InventoryTracker> InventoryTracker { get; set; }
        public DbSet<InventoryManagementAPI.Models.Role> AspNetRoles {  get; set; }
        public DbSet<InventoryManagementAPI.Models.User> Users {  get; set; }
        public DbSet<InventoryManagementAPI.Models.Log> Logs { get; set; }

        public async Task CreateDefaultSlot()
        {
            if(await Storages.AnyAsync())
            {
                return;
            }

            var defaultSlot = new Storage
            {
                Name = "Standardlager",
                CurrentStock = 50,
                MaxCapacity = null,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            Storages.Add(defaultSlot);
            await SaveChangesAsync();
        }
        public  async Task SeedRolesAndAdminUser(RoleManager<Role> roleManager, UserManager<User> userManager)
        {

            var roles = new[] { "Admin" };
            
            // Skapa roller
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new Models.Role
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        FullAccess = (role == "Admin"),
                        RoleName = role
                    };
                    await roleManager.CreateAsync(newRole);

                }
            }
           
            // Om admin inte finns, skapa rollen vid start
            var adminUserName = "AdminUser";
            var adminPassword = "AdminUser123!";
            string roleId = roleManager.Roles.FirstOrDefault()?.Id;
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new Models.User
                {
                    UserName = adminUserName,
                    RoleId = roleId,
                    EmployeeNumber = "0000",
                    FirstName = "Admin",
                    LastName = "User",
                    ProfilePic = "https://localhost:44353/images/profile1.png"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {                   
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        public async Task SeedTestDataAsync()
        {
            if (await Products.AnyAsync() && await Storages.AnyAsync())
            {
                return; 
            }

            var product = new Product
            {
                Name = "Testprodukt",
                ArticleNumber = "TP001",
                CurrentStock = 50,
                TotalStock = 50,
                Description = "Detta är en testprodukt.",
                Price = 99.99m,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            Products.Add(product);
            await SaveChangesAsync(); 

            var storage1 = new Storage
            {
                Name = "Lager 1",
                CurrentStock = 0,
                MaxCapacity = 200,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            var storage2 = new Storage
            {
                Name = "Lager 2",
                CurrentStock = 0,
                MaxCapacity = 200,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            Storages.AddRange(storage1, storage2);
            await SaveChangesAsync(); 

            var storageDefault = Storages.Where(x => x.Name == "Standardlager").FirstOrDefault();

            var inventoryTracker1 = new InventoryTracker
            {
                StorageId = storageDefault.Id,
                ProductId = product.Id,
                Quantity = 50
            };

            InventoryTracker.AddRange(inventoryTracker1);
            await SaveChangesAsync(); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
