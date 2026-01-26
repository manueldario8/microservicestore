using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;

namespace CatalogServiceAPI.Services
{
    public class ProviderService : IProviderRepository
    {
        public Task<Provider> CreateProviderAsync(Provider provider)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> GetProviderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Provider> UpdateProviderAsync(Provider provider)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ToggleStatusProviderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProviderAsync(int id)
        {
            throw new NotImplementedException();
        }     
        
    }
}
