using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTO;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementAPI.DAL
{
    public class UserManager
    {
        private readonly InventoryManagementAPIContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;
        public UserManager(InventoryManagementAPIContext context, UserManager<User> userManager)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _userManager = userManager;
        }
        public async Task CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();

            string firstTwoLettersFirstName = user.FirstName.Length >= 2 ? user.FirstName.Substring(0, 2).ToLower() : user.FirstName.ToLower();
            string firstTwoLettersLastName = user.LastName.Length >= 2 ? user.LastName.Substring(0, 2).ToLower() : user.LastName.ToLower();
            user.UserName = $"{firstTwoLettersFirstName}{firstTwoLettersLastName}{user.EmployeeNumber.ToLower()}";
            user.NormalizedUserName = $"{firstTwoLettersFirstName}{firstTwoLettersLastName}{user.EmployeeNumber.ToLower()}";
            user.Created = DateTime.Now;
            user.RoleId = null;
            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin123!");
            user.EmailConfirmed = false;
            user.ProfilePic = "https://localhost:44353/images/profile1.png";
            user.TwoFactorEnabled = false;
            await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(RoleDTO affectRole)
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
                userToUpdate.ProfilePic = user.ProfilePic;

                //Uppdaterar användarnamnet vid ändring av förnamn och/eller efternamn
                userToUpdate.UserName = user.UserName;
                userToUpdate.NormalizedUserName = user.NormalizedUserName ?? user.UserName.ToUpper();

                //Visar datumet för updaterad användare
                userToUpdate.Updated = DateTime.Now;

                userToUpdate.IsDeleted = user.IsDeleted;

                _context.Users.Update(userToUpdate);
                await _context.SaveChangesAsync();
            }
            else if (affectRole.ResetPassword == true)
            {
                // Hårdkodat lösenord vid reset
                var newPassword = "Admin123!";
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
                var resetResult = await _userManager.ResetPasswordAsync(userToUpdate, resetToken, newPassword);
            }
            else
            {

                var rolesToAffect = affectRole.CurrentRoles;
                var roleToAdd = affectRole.AddRole;
                if (affectRole.CurrentRoles != null)
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesToAffect);
                    userToUpdate.RoleId = null;
                    _context.Users.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                }
                else if (roleToAdd != null)
                {
                    var roleId = _context.Roles.FirstOrDefault(r => r.Name == roleToAdd);
                    await _userManager.AddToRoleAsync(userToUpdate, roleToAdd);
                    userToUpdate.RoleId = roleId.Id;
                    _context.Users.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<User> FindByIdAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<List<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }
    }
}
