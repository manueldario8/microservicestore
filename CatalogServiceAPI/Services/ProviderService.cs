using CatalogServiceAPI.Data;
using CatalogServiceAPI.DomainExceptions;
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
            await ValidateProviderAsync(dto.Name, dto.Code);

            var provider = new Provider
            {
                Name = dto.Name,
                Code = dto.Code
            };

            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();

            return new GetProviderCreatedDto(provider.Id,provider.Name,provider.Code);
        }

        public async Task<IEnumerable<GetProviderSimpleDto>> GetAllProvidersAsync()
        {
            return await _context.Providers
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Select(p => new GetProviderSimpleDto(p.Id, p.Name, p.Code, p.StatusActived))
                .ToListAsync();
        }

        public async Task<GetProviderWithProductsDto?> GetProviderByIdAsync(int id)
        {
            return await _context.Providers
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(p => p.Id == id)
                .Select(p => new GetProviderWithProductsDto(
                    p.Name,
                    p.Code,
                    p.Products.Select(d => new GetProductToCheckStockDto(
                        d.ProviderCode,
                        d.ProductCode, 
                        d.Name, 
                        d.Stock)),
                    p.StatusActived))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("No se encontró ningún proveedor con ese ID");
        }

        public async Task<GetUpdatedProviderDto> UpdateProviderAsync(int id, UpdateProviderDto dto)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new NotFoundException("No se encontró el proveedor");

            await ValidateProviderAsync(dto.Name, existing.Code, id);
            existing.Name = dto.Name;

            await _context.SaveChangesAsync();
            return new GetUpdatedProviderDto(existing.Code, dto.Name);
        }


        public async Task ToggleStatusProviderAsync(int id)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new NotFoundException("No se encontró el proveedor");
            existing.StatusActived = !existing.StatusActived;

            await _context.SaveChangesAsync();
        }



        //Internal functions
        private async Task ValidateProviderAsync(string name, string code, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("El nombre no puede estar vacío");
            if (string.IsNullOrWhiteSpace(code)) throw new InvalidOperationException("El código no puede estar vacío");

            var codeInUse = await _context.Providers.AnyAsync(p => p.Code.ToLower() == code.ToLower() && (!id.HasValue || p.Id != id.Value));


            if (codeInUse)
                throw new InvalidOperationException(
                    $"El código '{code}' ya está siendo usado por otro proveedor");
        }

    }
}
