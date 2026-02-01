using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using System.Numerics;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProviderService
    {
        Task <GetProviderSimpleDto> CreateProviderAsync(CreateProviderDto dto);
        Task <IEnumerable<GetProviderSimpleDto>> GetAllProvidersAsync();
        Task <GetProviderWithProductsDto?> GetProviderById(int id);
        Task <GetProviderSimpleDto> UpdateProviderAsync(int id, UpdateProviderDto dto);
        Task<bool> ToggleStatusProviderAsync(int id);
        Task DeleteProviderAsync(int id);

    }
}
