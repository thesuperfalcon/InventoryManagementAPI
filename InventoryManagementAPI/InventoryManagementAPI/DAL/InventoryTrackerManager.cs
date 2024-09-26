using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementAPI.DAL
{
    public class InventoryTrackerManager
    {

        private readonly InventoryManagementAPIContext _context;

        public InventoryTrackerManager(InventoryManagementAPIContext context)
        {
            _context = context;
        }

        public async Task UpdateInventoryTrackerAsync(int id, InventoryTracker tracker)
        {
            var existingTracker = await _context.InventoryTracker.FindAsync(id);
            var storage = await _context.Storages.FindAsync(tracker.StorageId);
            var product = await _context.Products.FindAsync(tracker.ProductId);

            if (existingTracker != null)
            {
                existingTracker.StorageId = tracker.StorageId;
                existingTracker.ProductId = tracker.ProductId;
                existingTracker.Quantity = tracker.Quantity;
                existingTracker.Modified = DateTime.Now;
                existingTracker.Product = product;
                existingTracker.Storage = storage;

                _context.InventoryTracker.Update(existingTracker);
                await _context.SaveChangesAsync();

                Console.WriteLine("har ändrats!!!!");

            }
        }
    }
}
