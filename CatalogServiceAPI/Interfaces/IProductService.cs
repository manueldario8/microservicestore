using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByCodes(string ProviderCode, string ProductCode);
        Task<Product?> GetProductById(int id);
        Task<Product> UpdateProductAsync(Product product);
        Task UpdateStockAsync(int id, int quantityDelta);
        Task<bool> ToggleStatusProduct(int id);
        Task DeleteProductAsync(int id);
        
    }
}
