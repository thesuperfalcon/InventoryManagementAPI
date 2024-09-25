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

namespace InventoryManagementAPI.Data
{
    public class InventoryManagementAPIContext : IdentityDbContext<User>
    {
        public InventoryManagementAPIContext (DbContextOptions<InventoryManagementAPIContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryManagementAPI.Models.Product> Products { get; set; }
        public DbSet<InventoryManagementAPI.Models.Storage> Storages { get; set; }
        public DbSet<InventoryManagementAPI.Models.Statistic> Statistics { get; set; } 
        public DbSet<InventoryManagementAPI.Models.ActivityLog> ActivityLog { get; set; }
        public DbSet<InventoryManagementAPI.Models.InventoryTracker> InventoryTracker { get; set; }

        public async Task CreateDefaultSlot()
        {
            if(await Storages.AnyAsync())
            {
                return;
            }

            var defaultSlot = new Storage
            {
                Name = "Default",
                CurrentStock = 50,
                MaxCapacity = null,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            Storages.Add(defaultSlot);
            await SaveChangesAsync();
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
                CurrentStock = 0,
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

            var storageDefault = Storages.Where(x => x.Name == "Default").FirstOrDefault();

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

            builder.Entity<Statistic>()
            .HasOne(s => s.InitialStorage)
            .WithMany(s => s.StatisticInitialStorages)
            .HasForeignKey(s => s.InitialStorageId);

            builder.Entity<Statistic>()
                .HasOne(s => s.DestinationStorage)
                .WithMany(s => s.StatisticDestinationStorages)
                .HasForeignKey(s => s.DestinationStorageId);

            builder.Entity<Statistic>()
                .HasOne(s => s.Product)
                .WithMany(s => s.Statistics)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();  

        }
    }
}
