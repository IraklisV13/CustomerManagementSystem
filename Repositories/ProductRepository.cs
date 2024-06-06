using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CMSContext _context;

        public ProductRepository(CMSContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductByExternalIdAsync(int externalId, int catalogId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ExternalId == externalId && p.ProductCatalogId == catalogId);
        }
    }
}
