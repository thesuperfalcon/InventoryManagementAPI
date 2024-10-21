using InventoryManagementAPI.DAL;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTO;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : Controller
    {

        private readonly InventoryManagementAPIContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly UserManager _userManager;


        public UsersController(InventoryManagementAPIContext context, UserManager userManager)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.User user)
        {
            await _userManager.CreateUser(user);
            
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] RoleDTO affectRole)
        {
            await _userManager.UpdateUser(affectRole);

            return Ok();
        }

        [HttpGet]
        public async Task<List<Models.User>> GetUsers()
        {

            List<Models.User> users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(string? inputValue)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                var searchTerms = inputValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x => x.IsDeleted == false && searchTerms.All(term =>
                x.FirstName.Contains(term) ||
                x.LastName.Contains(term) ||
                x.UserName.Contains(term) ||
                x.EmployeeNumber.Contains(term)));

                // lägg till fler searchTerms ifall man vill kunna söka fler atributer

            }

            var users = await query.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<Models.User> GetUserById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        [HttpGet("ExistingUsers")]
        public async Task<List<Models.User>> GetExistingUsers()
        {
            return await _context.Users.Where(x => x.IsDeleted == false).ToListAsync();
        }

        [HttpGet("DeletedUsers")]
        public async Task<List<Models.User>> GetDeletedUsers()
        {
            return await _context.Users.Where(x => x.IsDeleted == true).ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.IsDeleted == false)
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
            }
            else
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("UserRole/{id}")]
        public async Task<IActionResult> GetRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var roles = await _userManager.GetRolesAsync(user.Id);

            if (roles == null || !roles.Any())
            {
                return NotFound($"User with ID {id} is just a normal user.");
            }
            return Ok(roles);
        }
    }
}
