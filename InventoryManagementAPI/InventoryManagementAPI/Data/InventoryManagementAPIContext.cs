using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

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
