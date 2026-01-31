using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByCodesAsync(string ProviderCode, string ProductCode);
        Task<Product?> GetProductById(int id);
        Task<Product> UpdateProductAsync(int id, UpdateProductDto dto);
        Task UpdateStockAsync(int id, int quantityDelta);
        Task<bool> ToggleStatusProductAsync(int id);
        Task DeleteProductAsync(int id);
        
    }
}
