﻿using InventoryManagementAPI.Data;
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

            //Alicia: test för datum
            user.Created = DateTime.Now;

            user.RoleId = null;
            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin123!");
            user.EmailConfirmed = false;
            user.ProfilePic = "https://localhost:44353/images/profile1.png";
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
                userToUpdate.ProfilePic = user.ProfilePic;

                //Uppdaterar användarnamnet vid ändring av förnamn och/eller efternamn
                userToUpdate.UserName = user.UserName;
                userToUpdate.NormalizedUserName = user.NormalizedUserName ?? user.UserName.ToUpper();

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
        }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(string? name, string? employeeNumber)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => EF.Functions.Like((x.FirstName + " " + x.LastName).ToLower(), $"%{name.ToLower()}%"));
            }

            if (!string.IsNullOrEmpty(employeeNumber))
            {
                query = query.Where(x => EF.Functions.Like(x.EmployeeNumber, $"%{employeeNumber}%"));
            }


            //if (!string.IsNullOrEmpty(name))
            //{
            //    query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(lastName))
            //{
            //    query = query.Where(x => x.LastName.ToLower().Contains(lastName.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(employeeNumber))
            //{
            //    query = query.Where(x => x.EmployeeNumber.Contains(employeeNumber));
            //}


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
            if(user.IsDeleted == false)
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

    }
}
