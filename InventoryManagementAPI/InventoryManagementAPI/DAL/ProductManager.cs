using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
        public async Task SendProductToDefaultStorageAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            var defaultStorage = await _context.Storages.FirstOrDefaultAsync(x => x.Name == "Default");


            if(existingProduct != null)
            {
                var productTracker = new InventoryTracker
                {
                    StorageId = defaultStorage.Id,
                    ProductId = existingProduct.Id,
                    Quantity = existingProduct.TotalStock,
                    Modified = DateTime.Now,
                    Product = existingProduct,
                    Storage = defaultStorage,
                };
                defaultStorage.CurrentStock += existingProduct.TotalStock;
                _context.InventoryTracker.Update(productTracker);
                _context.Products.Update(existingProduct);
                _context.Storages.Update(defaultStorage);

                await _context.SaveChangesAsync();
            }
        }
    }
}
