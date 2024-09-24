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
using InventoryManagementAPI.DAL;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly InventoryManagementAPIContext _context;
        private readonly ProductManager _productManager;

        public ProductsController(InventoryManagementAPIContext context, ProductManager productManager)
        {
            _context = context;
            _productManager = productManager;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Product products)
        {
            _context.Products.Add(products);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Models.Product products)
        {
            await _productManager.UpdateProductAsync(id, products);
            return Ok();
        }

        [HttpGet]
        public async Task<List<Models.Product>> GetProducts()
        {
            List<Models.Product> products = await _context.Products.ToListAsync();
           
            return products;
        }
        [HttpGet("{id}")]
        public async Task<Models.Product> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            return product;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (!(bool)product.IsDeleted)
            {
                product.IsDeleted = true;
            }
            else
            {
                var statistics = await _context.Statistics.Where(x => x.ProductId == product.Id).ToListAsync();
                _context.Statistics.RemoveRange(statistics);
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}



