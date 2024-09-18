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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorages(int id)
        {
            var storage = await _context.Storages.FindAsync(id);
            if (storage == null)
            {
                return NotFound();
            }

            _context.Storages.Remove(storage);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
//        // GET: Storages
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Storage.ToListAsync());
//        }

//        // GET: Storages/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var storage = await _context.Storage
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (storage == null)
//            {
//                return NotFound();
//            }

//            return View(storage);
//        }

//        // GET: Storages/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Storages/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,MaxCapacity,CurrentStock,Created,Updated")] Storage storage)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(storage);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(storage);
//        }

//        // GET: Storages/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var storage = await _context.Storage.FindAsync(id);
//            if (storage == null)
//            {
//                return NotFound();
//            }
//            return View(storage);
//        }

//        // POST: Storages/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MaxCapacity,CurrentStock,Created,Updated")] Storage storage)
//        {
//            if (id != storage.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(storage);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!StorageExists(storage.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(storage);
//        }

//        // GET: Storages/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var storage = await _context.Storage
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (storage == null)
//            {
//                return NotFound();
//            }

//            return View(storage);
//        }

//        // POST: Storages/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var storage = await _context.Storage.FindAsync(id);
//            if (storage != null)
//            {
//                _context.Storage.Remove(storage);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool StorageExists(int id)
//        {
//            return _context.Storage.Any(e => e.Id == id);
//        }
//    }
//}
