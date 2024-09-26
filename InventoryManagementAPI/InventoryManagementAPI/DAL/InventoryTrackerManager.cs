using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
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
            }
        }

        public async Task<List<InventoryTracker>> GetTrackersByStorageAndProductIdAsync(int? productId, int? storageId)
        {
            List<InventoryTracker> inventoryTrackers = await _context.InventoryTracker.ToListAsync();

            List<InventoryTracker> selectedInventoryTrackers = new List<InventoryTracker>();


            switch (productId, storageId)
            {
                case (int inputProduct, int inputStorage) when inputProduct > 0 && inputStorage <= 0:
                    selectedInventoryTrackers = inventoryTrackers.Where(x => x.ProductId == inputProduct).ToList();
                    break;

                case (int inputProduct, int inputStorage) when inputStorage > 0 && inputProduct <= 0:
                    selectedInventoryTrackers = inventoryTrackers.Where(x => x.StorageId == inputStorage).ToList();
                    break;

                case (int inputProduct, int inputStorage) when inputProduct > 0 && inputStorage > 0:
                    selectedInventoryTrackers = inventoryTrackers.Where(x => x.StorageId == inputStorage && x.ProductId == inputProduct).ToList();
                    break;

                default:

                    break;
            }
            return selectedInventoryTrackers;
        }
    }
}
