using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly InventoryManagementAPIContext _context;

        public ProductsController(InventoryManagementAPIContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Product products)
        {
            _context.Products.Add(products);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Models.Product products)
        {
            _context.Update(products);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<List<Models.Product>> GetProducts()
        {
            List<Models.Product> products = await _context.Products.ToListAsync();
           
            return products;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}



