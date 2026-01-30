using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace CatalogServiceAPI.Services
{
    public class CategoryService(CatalogDbContext context) : ICategoryService
    {
        private readonly CatalogDbContext _context = context;


        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await ValidateCategoryAsync(category, isUpdated: false);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        } 

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            
            return await _context.Categories
                .AsNoTracking()
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Category> UpdateCategoryAsync(int id, string newName)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new InvalidOperationException($"Category not found.");

            existing.Name = newName;
            await ValidateCategoryAsync(existing, isUpdated: true);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new InvalidOperationException($"Category not found.");
            
                _context.Categories.Remove(existing);
                await _context.SaveChangesAsync();
            
        }


        private async Task ValidateCategoryAsync(Category category, bool isUpdated)
        {
            if (string.IsNullOrWhiteSpace(category.Name)) throw new InvalidOperationException("The field 'name' cannot be empty");

            var codeInUse = await _context.Categories.AnyAsync(p => p.Name.ToLower() == category.Name.ToLower() && (!isUpdated || p.Id != category.Id));


            if (codeInUse)
                throw new InvalidOperationException(
                    $"The name '{category.Name}' is already used.");
        }


    }
}
