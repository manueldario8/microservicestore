using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProviderRepository
    {
        Task <Provider> CreateProviderAsync(Provider provider);
        Task <IEnumerable<Provider>> GetAllProvidersAsync();
        Task <Provider?> GetProviderById(int id);
        Task <Provider> UpdateProviderAsync(Provider provider, string newName, string newCode);
        Task<bool> ToggleStatusProviderAsync(int id);
        Task DeleteProviderAsync(int id);

    }
}
