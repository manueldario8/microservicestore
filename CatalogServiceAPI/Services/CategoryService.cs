using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI.Services
{
    public class CategoryService(CatalogDbContext context) : ICategoryRepository
    {
        private readonly CatalogDbContext _context = context;


        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        } 

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            //var existing = await _context.Categories.FirstOrDefaultAsync(p=> p.Id == id) ?? throw new InvalidOperationException("Category not found");
            return await _context.Categories
               .Include(p => p.Products)
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Category> UpdateCategoryAsync(Category category, string newName)
        {
            var existing = await _context.Categories.FindAsync(category.Id) ?? throw new InvalidOperationException($"Category not found.");

            existing.Name = newName;
            _context.Entry(existing).CurrentValues.SetValues(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var existing = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"Category not found.");
            if (existing != null)
            {
                _context.Categories.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
