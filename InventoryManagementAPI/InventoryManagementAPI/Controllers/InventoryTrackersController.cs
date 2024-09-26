using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using InventoryManagementAPI.DTO;
using InventoryManagementAPI.DAL;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class InventoryTrackersController : Controller
    {
        private readonly InventoryManagementAPIContext _context;
        private readonly InventoryTrackerManager _trackerManager;

        public InventoryTrackersController(InventoryManagementAPIContext context, InventoryTrackerManager trackerManager)
        {
            _context = context;
            _trackerManager = trackerManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InventoryTrackerDto inventoryTrackerDto)
        {
            if (inventoryTrackerDto == null)
            {
                return BadRequest("Tracker data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var inventoryTracker = new Models.InventoryTracker
                {
                    StorageId = inventoryTrackerDto.StorageId,
                    ProductId = inventoryTrackerDto.ProductId,
                    Quantity = inventoryTrackerDto.Quantity
                };

                _context.InventoryTracker.Add(inventoryTracker);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventoryTrackerById), new { id = inventoryTracker.Id }, inventoryTracker);
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Internal server error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Models.InventoryTracker inventoryTracker)
        {

            await _trackerManager.UpdateInventoryTrackerAsync(id, inventoryTracker);
            return Ok();
        }

        [HttpGet("{productId?}/{storageId?}")]
        public async Task<IActionResult> GetTrackersByStorageAndProductId(int? productId, int? storageId)
        {

            var invntoryTracker = await _trackerManager.GetTrackersByStorageAndProductIdAsync(productId, storageId);
            if(invntoryTracker.Count <= 0)
            {
                return BadRequest("inga inventorytrackers hittades på dina input idn");
            }
            return Ok(invntoryTracker);
        }


        [HttpGet]
        public async Task<IActionResult> GetInventoryTracker()
        {
            var inventoryTracker = await _context.InventoryTracker
                .Include(x => x.Product)
                .Include(x => x.Storage)
                .ToListAsync();

            return Ok(inventoryTracker);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryTrackerById(int id)
        {
            var inventoryTracker = await _context.InventoryTracker
               .Include(x => x.Product)
               .Include(x => x.Storage)
               .Where(x => x.Id == id)
               .ToListAsync();

            return Ok(inventoryTracker);
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

