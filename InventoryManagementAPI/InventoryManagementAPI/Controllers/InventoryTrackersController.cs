using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;

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
		public async Task<IActionResult> GetInventoryTracker()
		{
			var inventoryTracker = await _context.InventoryTracker
				.Include(x => x.Product)
				.Include(x => x.Storage).ToListAsync();
			//An attempt to simplify the inventoryTracker
            var simplifiedResult = inventoryTracker.Select(it => new
            {
                it.Id,
                Product = new
                {
                    it.Product.Id,
                    it.Product.Name,
					it.Product.ArticleNumber,
					it.Product.CurrentStock,
					it.Product.TotalStock,
					it.Product.Description,
					it.Product.Price,
					it.Product.Created,
                    it.Storage.Updated

                    // Add other necessary product fields
                },
                Storage = new
                {
                    it.Storage.Id,
                    it.Storage.Name,
					it.Storage.Created,
					it.Storage.CurrentStock,
					it.Storage.MaxCapacity,
					it.Storage.Updated
                    // Add other necessary storage fields
                },
                it.Quantity
            });
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // For better readability
            };

            // Serialize with options
            var json = JsonSerializer.Serialize(inventoryTracker, options);
            Console.WriteLine(json);
            return Ok(simplifiedResult);
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

