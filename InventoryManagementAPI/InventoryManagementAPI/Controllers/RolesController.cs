using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RolesController : Controller
	{
		private readonly InventoryManagementAPIContext _context;

		public RolesController(InventoryManagementAPIContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<List<Models.Role>> GetRolesAsync()
		{
			List<Models.Role> roles = await _context.AspNetRoles.ToListAsync();

			return roles;
		}

    }
}
