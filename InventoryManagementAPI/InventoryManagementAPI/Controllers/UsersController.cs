using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : Controller
    {

        private readonly InventoryManagementAPIContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;

        public UsersController(InventoryManagementAPIContext context, UserManager<User> userManager )
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _userManager = userManager;

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.User user)
        {
            
            user.Id = Guid.NewGuid().ToString();
            user.UserName = user.EmployeeNumber;
            user.NormalizedUserName = user.EmployeeNumber;
            user.RoleId = null;
            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin123!");
            user.EmailConfirmed = false;
            user.TwoFactorEnabled = false;
            await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Models.User users)
        {
            _context.Update(users);
            await _context.SaveChangesAsync();
            return Ok();




        }

        [HttpGet]
        public async Task<List<Models.User>> GetUsers()
        {
            List<Models.User> users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
