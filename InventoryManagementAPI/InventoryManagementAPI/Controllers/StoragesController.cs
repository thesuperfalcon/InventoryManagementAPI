using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoragesController : Controller
    {
        private readonly InventoryManagementAPIContext _context;

        public StoragesController(InventoryManagementAPIContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Storage storages)
        {
            _context.Storages.Add(storages);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Models.Storage storages)
        {
            if (storages == null)
            {
                return BadRequest("Storage is null");
            }

            Console.WriteLine($"Updating Storage ID: {storages.Id}, CurrentStock: {storages.CurrentStock}");

            _context.Update(storages);
            await _context.SaveChangesAsync();
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
