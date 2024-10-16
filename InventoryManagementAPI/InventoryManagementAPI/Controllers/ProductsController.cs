using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;

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

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts(string? inputValue)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                var searchTerms = inputValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x => x.IsDeleted == false && (
                searchTerms.All(term => x.Name.Contains(term)) ||
                searchTerms.All(term => x.ArticleNumber.Contains(term)) ||
                searchTerms.All(term => x.Description.Contains(term)) ||
                searchTerms.All(term => x.Price.ToString().Contains(term)) ||
                searchTerms.All(term => x.TotalStock.ToString().Contains(term))));
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
        [HttpGet("ProductByName/{name}")]
        public async Task<bool> GetProductByName(string name)
        {
            var product = await _context.Products.Where(x => x.Name == name && x.IsDeleted == false).FirstOrDefaultAsync();
            return product != null ? true : false;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.IsDeleted == false)
            {
                product.IsDeleted = true;
                product.TotalStock = 0;
                product.CurrentStock = 0;
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



