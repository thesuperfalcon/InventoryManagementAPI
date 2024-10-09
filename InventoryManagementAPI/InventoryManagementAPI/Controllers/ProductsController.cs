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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            await _productManager.SendProductToDefaultStorageAsync(products.Id, products);
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
            return await _context.Products.ToListAsync();
        }
        [HttpGet("ByArticleNumber/{articleNumber}")]
        public async Task<Product> GetProductBySKU(string articleNumber)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ArticleNumber == articleNumber);
        }

        [HttpGet("{id}")]
        public async Task <Models.Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        [HttpGet("ExistingProducts")]
        public async Task<List<Models.Product>> GetExistingProducts()
        {
            return await _context.Products.Where(x => x.IsDeleted == false).ToListAsync();
        }

        [HttpGet("ExistingProducts/{id}")]
        public async Task<Models.Product> GetExistingProductById(int id)
        {
            return await _context.Products.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string? name, string? articleNumber)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(articleNumber))
            {
                query = query.Where(x => x.ArticleNumber.Contains(articleNumber));
            }

            var products = await query.ToListAsync();

            return Ok(products);
        }

        [HttpGet("DeletedProducts")]
        public async Task<List<Models.Product>> GetDeletedProducts()
        {
            return await _context.Products.Where(x => x.IsDeleted == true).ToListAsync();

        }

        [HttpGet("DeletedProducts/{id}")]
        public async Task<Models.Product> GetDeletedProductById(int id)
        {
            return await _context.Products.Where(x => x.IsDeleted == true).FirstOrDefaultAsync(x => x.Id == id);
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
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}



