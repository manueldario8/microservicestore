using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProductService
    {
        //To administrators
        Task<CreatedProductDto> CreateProductAsync(CreateProductDto dto, string? urlPhoto);
        Task<IEnumerable<GetProductToListByAdminDto>> GetAllProductsByAdminAsync();
        Task<GetProductToSellDto?> GetProductByAdminByCodesAsync(string ProviderCode, string ProductCode);
        Task<GetProductToListByAdminDto?> GetProductByAdminById(int id);
        Task<GetProductToListByAdminDto> UpdateProductAsync(int id, UpdateProductDto dto);
        Task UpdateStockAsync(int id, int quantityDelta);
        Task<bool> ToggleStatusProductAsync(int id);
        Task DeleteProductAsync(int id);

        //To clients
        Task<IEnumerable<GetProductToListByClientDto>> GetAllProductsByClientAsync();
        Task<GetProductToOrderClientDto?> GetProductByClientByCodesAsync(string ProviderCode, string ProductCode);
        
    }
}
