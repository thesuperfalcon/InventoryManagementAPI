using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.DAL;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoragesController : Controller
    {
        private readonly InventoryManagementAPIContext _context;
        private readonly StorageManager _storageManager;

        public StoragesController(InventoryManagementAPIContext context, StorageManager storageManager)
        {
            _context = context;
            _storageManager = storageManager;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Storage storages)
        {
            _context.Storages.Add(storages);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Models.Storage storages)
        {
            await _storageManager.UpdateStoragesAsync(id, storages);
            return Ok();
        }

        [HttpGet("DefaultStorage")]
        public async Task <Storage> GetDefaultStorageAsync()
        {
            return await _context.Storages.Where(x => x.Name == "Standardlager").FirstOrDefaultAsync();
        }

        [HttpGet]
        public async Task<List<Models.Storage>> GetStorages()
        {
            var storages = await _context.Storages.Include(x => x.InventoryTrackers).ThenInclude(z => z.Product).ToListAsync();
            return storages;
        }

        [HttpGet("ByStorageName/{storageName}")]
        public async Task<Models.Storage> GetStorageByNameAsync(string storageName)
        {
            return await _context.Storages.FirstOrDefaultAsync(x => x.Name == storageName);
        }

        [HttpGet("{id}")]
        public async Task<Models.Storage> GetStorageById(int id)
        {
            return await _context.Storages.FindAsync(id);
        }

        [HttpGet("ExistingStorages")]
        public async Task<List<Models.Storage>> GetExistingStorages()
        {
            var existingStorages = await _context.Storages.Where(x => x.IsDeleted == false).Include(z => z.InventoryTrackers).ThenInclude(x => x.Product).ToListAsync();
            return existingStorages;        
        }

        [HttpGet("ExistingStorages/{id}")]
        public async Task<Models.Storage> GetExistingStorageById(int id)
        {
            return await _context.Storages.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("DeletedStorages")]
        public async Task<List<Models.Storage>> GetDeletedStorages()
        {
            return await _context.Storages.Where(x => x.IsDeleted == true).ToListAsync();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchStorages(string? name)
        {
            var query = _context.Storages.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var storages = await query.ToListAsync();

            return Ok(storages);
        }

        [HttpGet("DeletedStorages/{id}")]
        public async Task<Models.Storage> GetDeletedStorageById(int id)
        {
            return await _context.Storages.Where(x => x.IsDeleted == true).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorages(int id)
        {
            var storage = await _context.Storages.FindAsync(id);

            if (storage == null)
            {
                return NotFound();
            }

            if (!(bool)storage.IsDeleted)
            {
                storage.IsDeleted = true;
            }
            else
            {
                _context.Storages.Remove(storage);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
