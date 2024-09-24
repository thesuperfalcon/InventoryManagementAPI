using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.DAL
{
    public class ProductManager
    {
        private readonly InventoryManagementAPIContext _context;
        public ProductManager(InventoryManagementAPIContext context)
        {
            _context = context;
        }
        public async Task UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.ArticleNumber = product.ArticleNumber;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.TotalStock = product.TotalStock;
                existingProduct.CurrentStock = product.CurrentStock;
                existingProduct.Created = product.Created;
                existingProduct.Updated = DateTime.Now;
                existingProduct.IsDeleted = product.IsDeleted;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
        }
    }
}
