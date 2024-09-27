using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
		public async Task<List<IdentityRole>> GetRolesAsync()
		{
			List<IdentityRole> roles = await _context.Roles.ToListAsync();
			return roles;
		}

	}
}
