using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class ProviderService(CatalogDbContext context) : IProviderService
    {
        private readonly CatalogDbContext _context = context;


        public async Task<Provider> CreateProviderAsync(Provider provider)
        {
            await ValidateProviderAsync(provider, isUpdated: false);

            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            return await _context.Providers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Provider?> GetProviderById(int id)
        {
            return await _context.Providers
                .AsNoTracking()
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Provider> UpdateProviderAsync(int id, string newName)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new InvalidOperationException("Provider not found.");

            existing.Name = newName;

            await ValidateProviderAsync(existing, isUpdated: true);

            await _context.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> ToggleStatusProviderAsync(int id)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new InvalidOperationException($"Provider not found.");

            existing.StatusActived = !existing.StatusActived;

            await _context.SaveChangesAsync();
            return existing.StatusActived;
        }

        public async Task DeleteProviderAsync(int id)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new InvalidOperationException($"Provider not found.");
            
                _context.Providers.Remove(existing);
                await _context.SaveChangesAsync();
            
        }



        private async Task ValidateProviderAsync(Provider provider, bool isUpdated)
        {
            if (string.IsNullOrWhiteSpace(provider.Name)) throw new InvalidOperationException("The field 'name' cannot be empty");
            if (string.IsNullOrWhiteSpace(provider.Code)) throw new InvalidOperationException("The field 'code' cannot be empty");

            var codeInUse = await _context.Providers.AnyAsync(p => p.Code.ToLower() == provider.Code.ToLower() && (!isUpdated || p.Id != provider.Id));


            if (codeInUse)
                throw new InvalidOperationException(
                    $"The provider code '{provider.Code}' is already used.");
        }


    }
}
