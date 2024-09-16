using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]

	public class InventoryTrackersController : Controller
	{
		private readonly InventoryManagementAPIContext _context;

		public InventoryTrackersController(InventoryManagementAPIContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Models.InventoryTracker inventoryTracker)
		{
			_context.InventoryTracker.Add(inventoryTracker);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] Models.InventoryTracker inventoryTracker)
		{
			_context.Update(inventoryTracker);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet]
		public async Task<List<Models.InventoryTracker>> GetInventoryTracker()
		{
			List<Models.InventoryTracker> inventoryTracker = await _context.InventoryTracker.ToListAsync();
			return inventoryTracker;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInventoryTracker(int id)
		{
			var inventoryTracker = await _context.InventoryTracker.FindAsync(id);
			if (inventoryTracker == null)
			{
				return NotFound();
			}

			_context.InventoryTracker.Remove(inventoryTracker);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}

}

