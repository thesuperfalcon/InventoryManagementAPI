using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Data
{
    public class InventoryManagementAPIContext : DbContext
    {
        public InventoryManagementAPIContext (DbContextOptions<InventoryManagementAPIContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryManagementAPI.Models.Product> Products { get; set; } = default!;
        public DbSet<InventoryManagementAPI.Models.Storage> Storages { get; set; } = default!;

        public DbSet<InventoryManagementAPI.Models.User> Users { get; set; } = default!;
    }
}
