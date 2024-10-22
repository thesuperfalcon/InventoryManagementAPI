using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class InventoryManagementAPIContext : IdentityDbContext<User, Models.Role, string>
    {


        public InventoryManagementAPIContext(DbContextOptions<InventoryManagementAPIContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryManagementAPI.Models.Product> Products { get; set; }
        public DbSet<InventoryManagementAPI.Models.Storage> Storages { get; set; }
        public DbSet<InventoryManagementAPI.Models.Statistic> Statistics { get; set; }
        public DbSet<InventoryManagementAPI.Models.InventoryTracker> InventoryTracker { get; set; }
        public DbSet<InventoryManagementAPI.Models.Role> AspNetRoles { get; set; }
        public DbSet<InventoryManagementAPI.Models.User> Users { get; set; }
        public DbSet<InventoryManagementAPI.Models.Log> Logs { get; set; }

        public DbSet<InventoryManagementAPI.Models.Developer> Developers { get; set; }

        public async Task CreateDefaultSlot()
        {
            if (await Storages.AnyAsync())
            {
                return;
            }

            var defaultSlot = new Storage
            {
                Name = "Standardlager",
                CurrentStock = 0,
                MaxCapacity = null,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };

            Storages.Add(defaultSlot);
            await SaveChangesAsync();
        }

        public async Task SeedDevelopers()
        {
            if (await Developers.AnyAsync())
            {
                return;
            }

            var developers = new List<Developer>
            {
                new Developer
                {
                    Name = "Gustav Berg",
                    ImageUrl = "gb.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/gurranb",
                    LinkedInUrl = "https://www.linkedin.com/in/gustav-berg7/"
                },
                new Developer
                {
                    Name = "Tim Tallberg",
                    ImageUrl = "tt.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/Tallis92",
                    LinkedInUrl = "https://www.linkedin.com/in/tim-tallberg/"
                },
                new Developer
                {
                    Name = "Alicia Blomqvist",
                    ImageUrl = "ab.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/aliciablomqvist",
                    LinkedInUrl = "https://www.linkedin.com/in/alicia-blomqvist-43624a242/"
                },
                new Developer
                {
                    Name = "Tintin Falk",
                    ImageUrl = "tf.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/thesuperfalcon",
                    LinkedInUrl = "https://www.linkedin.com/in/tintinfalk/"
                },
                new Developer
                {
                    Name = "Elin Sand",
                    ImageUrl = "es1.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/ElinSand",
                    LinkedInUrl = "https://www.linkedin.com/in/elin-sand"
                },             
                new Developer
                {
                    Name = "Robin Jonsson",
                    ImageUrl = "rj.jpg",
                    Title = "Lia student",
                    GithubUrl = "https://github.com/ilhobbito",
                    LinkedInUrl = "https://www.linkedin.com/in/jonsson-robin/"
                }
            };

            Developers.AddRange(developers);
            await SaveChangesAsync();
        }

        public async Task SeedRolesAndAdminUser(RoleManager<Role> roleManager, UserManager<User> userManager)
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
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    ProfilePic = "https://localhost:44353/images/profile1.png"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
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
