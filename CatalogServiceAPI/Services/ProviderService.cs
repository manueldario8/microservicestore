using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class ProviderService(CatalogDbContext context) : IProviderRepository
    {
        private readonly CatalogDbContext _context = context;


        public async Task<Provider> CreateProviderAsync(Provider provider)
        {
            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            return await _context.Providers.ToListAsync();
        }

        public async Task<Provider?> GetProviderById(int id)
        {
            return await _context.Providers
               .Include(p => p.Products)
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Provider> UpdateProviderAsync(Provider provider, string newName, string newCode)
        {
            var existing = await _context.Providers.FindAsync(provider.Id) ?? throw new InvalidOperationException($"Provider not found.");

            existing.Name = newName;
            existing.Code = newCode;
            await _context.SaveChangesAsync();

            //Keep maintance with the same code string
            var codeInUse = await _context.Providers.AnyAsync(p => p.Code == existing.Code && p.Id != existing.Id);

            if (codeInUse)
                throw new InvalidOperationException($"The provider code '{existing.Code}' is already used.");

            _context.Entry(existing).CurrentValues.SetValues(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> ToggleStatusProviderAsync(int id)
        {
            var existing = await _context.Providers.FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"Provider not found.");
            existing.StatusActived = !existing.StatusActived;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteProviderAsync(int id)
        {
            var existing = await _context.Providers.FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"Provider not found.");
            if (existing != null)
            {
                _context.Providers.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }     
        
    }
}
