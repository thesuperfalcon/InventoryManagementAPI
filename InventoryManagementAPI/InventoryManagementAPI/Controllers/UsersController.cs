using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTO;
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

            string firstTwoLettersFirstName = user.FirstName.Length >= 2 ? user.FirstName.Substring(0, 2).ToLower() : user.FirstName.ToLower();
            string firstTwoLettersLastName = user.LastName.Length >= 2 ? user.LastName.Substring(0, 2).ToLower() : user.LastName.ToLower();
            user.UserName = $"{firstTwoLettersFirstName}{firstTwoLettersLastName}{user.EmployeeNumber.ToLower()}";
            //user.UserName = user.EmployeeNumber;
            user.NormalizedUserName = $"{firstTwoLettersFirstName}{firstTwoLettersLastName}{user.EmployeeNumber.ToLower()}";
            //user.NormalizedUserName = user.EmployeeNumber;
            user.RoleId = null;
            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin123!");
            user.EmailConfirmed = false;
            user.TwoFactorEnabled = false;
            await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] RoleDTO affectRole)
        {
            var user = affectRole.User;

            // Av någon anledning sparar den inte in i databasen om jag använder user direkt utan måste hämta userToUpdate 
            // och uppdatera genom den variabeln istället

            var userToUpdate = await _context.Users.FindAsync(user.Id);
            if (affectRole.CurrentRoles == null && affectRole.AddRole == null && affectRole.ResetPassword == false)
            {
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.EmployeeNumber = user.EmployeeNumber;

                //Visar datumet för updaterad användare
                userToUpdate.Updated = DateTime.Now;

                _context.Users.Update(userToUpdate);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else if(affectRole.ResetPassword == true)
            {
                // Hårdkodat lösenord vid reset
                var newPassword = "Admin123!";
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
                var resetResult = await _userManager.ResetPasswordAsync(userToUpdate, resetToken, newPassword);
                return Ok();
            }
            else
            {
                
                var rolesToAffect = affectRole.CurrentRoles;
                var roleToAdd = affectRole.AddRole;
                if(affectRole.CurrentRoles != null)
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesToAffect);
                    userToUpdate.RoleId = null;
                    _context.Users.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else if(roleToAdd != null)
                {
                    var roleId = _context.Roles.FirstOrDefault(r => r.Name == roleToAdd);
                    await _userManager.AddToRoleAsync(userToUpdate, roleToAdd);
                    userToUpdate.RoleId = roleId.Id;
                    _context.Users.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return Ok();                
            }
        }

        [HttpGet]
        public async Task<List<Models.User>> GetUsers()
        {
            List<Models.User> users = await _context.Users.ToListAsync();
            return users;
   //         return users.Select(u => new Models.User
			//{
   //             Id = u.Id,
   //             FirstName = u.FirstName,
   //             LastName = u.LastName,
   //             EmployeeNumber = u.EmployeeNumber,
   //             RoleId = u.RoleId,
   //             Created = u.Created,
   //             Updated = u.Updated


   //         }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<Models.User> GetUserById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
