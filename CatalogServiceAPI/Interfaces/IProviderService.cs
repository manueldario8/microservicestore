using CatalogServiceAPI.Entities.Models;
using System.Numerics;

namespace CatalogServiceAPI.Interfaces
{
    public interface IProviderService
    {
        Task <Provider> CreateProviderAsync(Provider provider);
        Task <IEnumerable<Provider>> GetAllProvidersAsync();
        Task <Provider?> GetProviderById(int id);
        Task <Provider> UpdateProviderAsync(int id, string newName);
        Task<bool> ToggleStatusProviderAsync(int id);
        Task DeleteProviderAsync(int id);

    }
}
