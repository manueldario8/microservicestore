using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class ProductService(CatalogDbContext context) : IProductService

    {
        private readonly CatalogDbContext _context = context;


        public async Task<Product> CreateProductAsync(Product product)
        {

            await ValidateProductAsync(product, isUpdate: false);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;

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

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existing = await _context.Products
                .FindAsync(product.Id)?? throw new InvalidOperationException("Product not found.");

            await ValidateProductAsync(product, isUpdate: true);

            await _context.SaveChangesAsync();

            return existing;
        }


        public async Task<Product?> GetProductByCodes(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProviderCode == providerCode && p.ProductCode == productCode);
        }

        public async Task<bool> ToggleStatusProduct(int id)
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


        private async Task ValidateProductAsync(Product product, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new InvalidOperationException("Product name is required.");

            if (string.IsNullOrWhiteSpace(product.ProductCode))
                throw new InvalidOperationException("Product code is required.");

            if (string.IsNullOrWhiteSpace(product.ProviderCode))
                throw new InvalidOperationException("Provider code is required.");

            if (product.Price <= 0)
                throw new InvalidOperationException("Price must be greater than zero.");

            var codeInUse = await _context.Products.AnyAsync(p =>
                p.ProductCode == product.ProductCode &&
                p.ProviderCode == product.ProviderCode &&
                (!isUpdate || p.Id != product.Id));

            if (codeInUse)
                throw new InvalidOperationException(
                    $"The product code '{product.ProductCode}' is already used for this provider.");
        }
    }
}
