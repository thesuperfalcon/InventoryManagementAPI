using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
       
            private readonly InventoryManagementAPIContext _context;

            public UsersController(InventoryManagementAPIContext context)
            {
                _context = context;
            }
            [HttpPost]
            public async Task<IActionResult> Post([FromBody] Models.User users)
            {
                _context.Users.Add(users);
                users.Id = Guid.NewGuid().ToString();
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
