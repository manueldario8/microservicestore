using CatalogServiceAPI.Data;
using CatalogServiceAPI.DomainExceptions;
using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class ProductService(CatalogDbContext context) : IProductService

    {
        private readonly CatalogDbContext _context = context;

        /*Services to administrator*/
        public async Task<CreatedProductDto> CreateProductAsync(CreateProductDto dto, string? urlPhoto)
        {

            await ValidateProductToCreateAsync(dto);
            var product = new Product { 
                ProviderCode = dto.ProviderCode,
                ProductCode = dto.ProductCode,
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
                UrlPhoto = urlPhoto
            };

            var category = await _context.Categories
                .Where(c => c.Id == dto.CategoryId)
                .Select(c => new GetCategorySimpleByClientDto(c.Name))
                .FirstAsync();


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new CreatedProductDto(
                product.Id,
                product.ProviderCode,
                product.ProductCode,
                category,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.UrlPhoto);
        }

        public async Task<IEnumerable<GetProductToListByAdminDto>> GetAllProductsByAdminAsync()
        {
            return await _context.Products
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(p => new GetProductToListByAdminDto(
                    p.ProviderCode, 
                    p.ProductCode, 
                    new GetCategorySimpleByClientDto(
                        p.Category.Name), 
                    p.Name, 
                    p.Price, 
                    p.Stock))
                .ToListAsync();
        }

        public async Task<GetProductToListByAdminDto?> GetProductByAdminById(int id)
        {
            return await _context.Products
               .IgnoreQueryFilters()
               .AsNoTracking()
               .Where(p => p.Id == id)
               .Select(p => new GetProductToListByAdminDto(
                   p.ProviderCode, 
                   p.ProductCode, 
                   new GetCategorySimpleByClientDto(
                        p.Category.Name), 
                   p.Name, 
                   p.Price, 
                   p.Stock))
               .FirstOrDefaultAsync() ?? throw new NotFoundException("ID de Producto no encontrado");
        }
        
        public async Task<GetProductToSellDto?> GetProductByAdminByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProviderCode == providerCode && p.ProductCode == productCode)
                .Select(p => new GetProductToSellDto(
                    p.ProviderCode,
                    p.ProductCode,
                    p.Name,
                    p.Price))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Producto no encontrado");
        }
        public async Task<GetProductToListByAdminDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            await ValidateProductToUpdateAsync(dto);

            var existing = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new NotFoundException("Producto no encontrado");

            existing.Name = dto.Name;
            existing.CategoryId = dto.CategoryId;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.UrlPhoto = dto.UrlPhoto;

            await _context.SaveChangesAsync();
            await _context.Entry(existing)
            .Reference(p => p.Category)
            .LoadAsync();

            return new GetProductToListByAdminDto(
                existing.Provider.Code,
                existing.ProductCode,
                new GetCategorySimpleByClientDto(
                    existing.Category.Name
                ),
                existing.Name,
                existing.Price,
                existing.Stock
            );
        }

        public async Task ToggleStatusProductAsync(int id)
        {
            var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException($"Product not found");

            product.StatusActived = !product.StatusActived;
            await _context.SaveChangesAsync();
        }
      
        public Task UpdateStockAsync(int id, int quantityDelta)
        {
            throw new NotImplementedException();
        }

        /*public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new InvalidOperationException($"Product not found");

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }*/


        /*Service to clientside*/
        public async Task<IEnumerable<GetProductToListByClientDto>> GetAllProductsByClientAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetProductToListByClientDto(
                    p.ProviderCode, 
                    p.ProductCode, 
                    new GetCategorySimpleByClientDto(
                        p.Category.Name), 
                    p.Name, 
                    p.Description, 
                    p.Price, 
                    p.UrlPhoto))
                .ToListAsync();
        }

        public async Task<GetProductToOrderClientDto?> GetProductByClientByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProviderCode == providerCode && p.ProductCode == productCode)
                .Select(p => new GetProductToOrderClientDto(
                    p.ProviderCode,
                    p.ProductCode,
                    p.Name,
                    p.Price))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Producto no encontrado");
        }


        //Internal functions
        private async Task ValidateProductToCreateAsync(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProviderCode))
                throw new BusinessException("Se necesita un código de proveedor");

            if (string.IsNullOrWhiteSpace(dto.ProductCode))
                throw new ("Se necesita un código de identificación para el producto");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessException("Se necesita el nombre del producto");

            if (dto.Price <= 0)
                throw new BusinessException("El stock no puede ser negativo al crearse el producto");


            var providerExists = await _context.Providers
                .AnyAsync(p => p.Code == dto.ProviderCode);
            if (!providerExists)
                throw new InvalidOperationException($"No existe ningún proveedor con el código {dto.ProviderCode}");


            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new InvalidOperationException("La categoría no existe o está desactivada");


            var codeInUse = await _context.Products.AnyAsync(p =>
                p.ProductCode == dto.ProductCode &&
                p.ProviderCode== dto.ProviderCode);

            if (codeInUse)
                throw new BusinessException(
                    $"El código '{dto.ProductCode}' ya está asignado a otro producto de este proveedor.");
        }

        private async Task ValidateProductToUpdateAsync(UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("El producto debe tener nombre.");

            if (dto.Price <= 0)
                throw new InvalidOperationException("El precio no puede ser negativo o cero");
            if (dto.Stock < 0)
                throw new InvalidOperationException("El stock debe ser mayor o igual que cero");
           
        }

    }
}
