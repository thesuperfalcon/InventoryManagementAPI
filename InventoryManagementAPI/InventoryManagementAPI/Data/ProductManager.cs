using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class ProductManager
    {
        private readonly InventoryManagementAPIContext _context;

        public ProductManager(InventoryManagementAPIContext context)
        {
            _context = context;
        }

        public async Task AddProduct(InventoryManagementAPI.Models.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Models.Product>> GetProducts()
        {
            List<Models.Product> products = await _context.Products.ToListAsync();

            return products;
        }
    public async Task AddAsync(Models.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
    }
}
