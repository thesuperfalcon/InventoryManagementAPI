using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
    }
}
