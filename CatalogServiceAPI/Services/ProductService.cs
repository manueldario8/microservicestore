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


        public async Task<GetProductToListByAdminDto> CreateProductAsync(CreateProductDto dto)
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

            return new GetProductToListByAdminDto(dto.ProviderCode, dto.ProductCode, category, dto.Name, dto.Price, dto.Stock);

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
               .AsNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var existing = await _context.Products
                .FirstOrDefaultAsync(p=>p.Id == id)?? throw new InvalidOperationException("Product not found.");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.UrlPhoto = dto.UrlPhoto;

            await ValidateProductAsync(existing, isUpdate: true);

            await _context.SaveChangesAsync();

            return existing;
        }


        public async Task<Product?> GetProductByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProviderCode == providerCode && p.ProductCode == productCode);
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
