using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

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
			var query = _context.InventoryTracker.AsQueryable();

			query = query.Include(x => x.Product)
						 .Include(x => x.Storage);

			if (productId.HasValue && productId > 0)
			{
				query = query.Where(x => x.ProductId == productId);
			}

			if (storageId.HasValue && storageId > 0)
			{
				query = query.Where(x => x.StorageId == storageId);
			}

			return await query.ToListAsync();
		}
	}
}
