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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put([FromBody] Models.Storage storages)
        //{
        //    _context.Update(storages);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Models.Storage storages)
        {
            await _storageManager.UpdateStoragesAsync(id, storages);
            return Ok();
        }


        [HttpGet]
        public async Task<List<Models.Storage>> GetStorages()
        {
            List<Models.Storage> storages = await _context.Storages.ToListAsync();
            return storages;
        }
        [HttpGet("{id}")]
        public async Task<Models.Storage> GetStorageById(int id)
        {
            return await _context.Storages.FindAsync(id);
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
