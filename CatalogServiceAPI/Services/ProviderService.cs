using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class ProviderService(CatalogDbContext context) : IProviderService
    {
        private readonly CatalogDbContext _context = context;

        public async Task<GetProviderCreatedDto> CreateProviderAsync(CreateProviderDto dto)
        {
            await ValidateProviderAsync(dto.Code, dto.Name);

            var provider = new Provider
            {
                Code = dto.Code,
                Name = dto.Name
            };

            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();

            return new GetProviderCreatedDto(provider.Id,provider.Code,provider.Name);
        }

        public async Task<IEnumerable<GetProviderSimpleDto>> GetAllProvidersAsync()
        {
            return await _context.Providers
                .AsNoTracking()
                .Select(p => new GetProviderSimpleDto(p.Code, p.Name))
                .ToListAsync();
        }

        public async Task<GetProviderWithProductsDto?> GetProviderByIdAsync(int id)
        {
            return await _context.Providers
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new GetProviderWithProductsDto(
                    p.Code, 
                    p.Name, 
                    p.Products.Select(d => new GetProductToSellDto(
                        d.ProviderCode,
                        d.ProductCode, 
                        d.Name, 
                        d.Price))))
                .FirstOrDefaultAsync();
        }

        public async Task<GetProviderSimpleDto> UpdateProviderAsync(int id, UpdateProviderDto dto)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new InvalidOperationException("Provider not found.");

            await ValidateProviderAsync(existing.Code, dto.Name, id);

            existing.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new GetProviderSimpleDto(dto.Name, existing.Code);
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


        //Internal functions
        private async Task ValidateProviderAsync(string code, string name, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("The field 'name' cannot be empty");
            if (string.IsNullOrWhiteSpace(code)) throw new InvalidOperationException("The field 'code' cannot be empty");

            var codeInUse = await _context.Providers.AnyAsync(p => p.Code.ToLower() == code.ToLower() && (!id.HasValue || p.Id != id.Value));


            if (codeInUse)
                throw new InvalidOperationException(
                    $"The provider code '{code}' is already used.");
        }

    }
}
