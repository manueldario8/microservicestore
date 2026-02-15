using CatalogServiceAPI.Data;
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
        public async Task<CreatedProductDto> CreateProductAsync(CreateProductDto dto)
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
                UrlPhoto = dto.UrlPhoto
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
                .AsNoTracking()
                .Select(p => new GetProductToListByAdminDto(p.ProviderCode, p.ProductCode, new GetCategorySimpleByClientDto(
                p.Category.Name
            ), p.Name, p.Price, p.Stock))
                .ToListAsync();
        }

        public async Task<GetProductToListByAdminDto?> GetProductByAdminById(int id)
        {
            return await _context.Products
               .AsNoTracking()
               .Where(p => p.Id == id)
               .Select(p => new GetProductToListByAdminDto(p.ProviderCode, p.ProductCode, new GetCategorySimpleByClientDto(
                p.Category.Name), p.Name, p.Price, p.Stock))
               .FirstOrDefaultAsync();
        }
        
        public async Task<GetProductToSellDto?> GetProductByAdminByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetProductToSellDto(p.ProviderCode, p.ProductCode, p.Name, p.Price))
                .FirstOrDefaultAsync(p => p.ProviderCode == providerCode && p.ProductCode == productCode);
        }
        public async Task<GetProductToListByAdminDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            await ValidateProductToUpdateAsync(dto);

            var existing = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Product not found.");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.UrlPhoto = dto.UrlPhoto;

            await _context.SaveChangesAsync();

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

        public async Task<bool> ToggleStatusProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new InvalidOperationException($"Product not found");

            product.StatusActive = !product.StatusActive;
            
            await _context.SaveChangesAsync();

            return product.StatusActive;
        }
      
        public Task UpdateStockAsync(int id, int quantityDelta)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new InvalidOperationException($"Product not found");

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }


        /*Service to clientside*/
        public async Task<IEnumerable<GetProductToListByClientDto>> GetAllProductsByClientAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetProductToListByClientDto(p.ProviderCode, p.ProductCode, new GetCategorySimpleByClientDto(
                p.Category.Name
            ), p.Name, p.Description, p.Price, p.UrlPhoto))
                .ToListAsync();
        }

        public async Task<GetProductToOrderClientDto?> GetProductByClientByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetProductToOrderClientDto(p.ProviderCode, p.ProductCode, p.Name, p.Price))
                .FirstOrDefaultAsync(p => p.ProviderCode == providerCode && p.ProductCode == productCode);
        }


        //Internal functions
        private async Task ValidateProductToCreateAsync(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProviderCode))
                throw new InvalidOperationException("Provider code is required.");

            if (string.IsNullOrWhiteSpace(dto.ProductCode))
                throw new InvalidOperationException("Product code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("Product name is required.");

            if (dto.Price <= 0)
                throw new InvalidOperationException("Price must be greater than zero.");


            var providerExists = await _context.Providers
                .AnyAsync(p => p.Code == dto.ProviderCode);
            if (!providerExists)
                throw new InvalidOperationException("Provider does not exist.");


            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new InvalidOperationException("Category does not exist.");


            var codeInUse = await _context.Products.AnyAsync(p =>
                p.ProductCode == dto.ProductCode &&
                p.ProviderCode== dto.ProviderCode);

            if (codeInUse)
                throw new InvalidOperationException(
                    $"The product code '{dto.ProductCode}' is already used for this provider.");
        }

        private async Task ValidateProductToUpdateAsync(UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("Product name is required.");

            if (dto.Price <= 0)
                throw new InvalidOperationException("Price must be greater than zero.");
            if (dto.Stock < 0)
                throw new InvalidOperationException("Stock must be greater or equal to zero.");
           
        }

    }
}
