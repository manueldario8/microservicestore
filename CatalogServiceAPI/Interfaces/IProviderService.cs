using CatalogServiceAPI.Entities.DTOs;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProviderService
    {
        Task <GetProviderCreatedDto> CreateProviderAsync(CreateProviderDto dto);
        Task <IEnumerable<GetProviderSimpleDto>> GetAllProvidersAsync();
        Task <GetProviderWithProductsDto?> GetProviderByIdAsync(int id);
        Task <GetProviderSimpleDto> UpdateProviderAsync(int id, UpdateProviderDto dto);
        Task<bool> ToggleStatusProviderAsync(int id);
        Task DeleteProviderAsync(int id);
    }
}
