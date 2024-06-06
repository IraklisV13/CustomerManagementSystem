using CustomerManagementSystem.Models;

namespace CustomerManagementSystem.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<Product> GetProductByExternalIdAsync(int externalId, int catalogId);
    }
}
