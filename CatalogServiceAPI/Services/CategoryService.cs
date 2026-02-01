using CatalogServiceAPI.Data;
using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace CatalogServiceAPI.Services
{
    public class CategoryService(CatalogDbContext context) : ICategoryService
    {
        private readonly CatalogDbContext _context = context;

        //Tasks to Administrators
        public async Task<GetCategorySimpleByAdminDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            await ValidateCategoryAsync(dto.Name);


            var category = new Category
            {
                Name = dto.Name
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new GetCategorySimpleByAdminDto(category.Id, category.Name);
        } 

        public async Task<IEnumerable<GetCategorySimpleByAdminDto>> GetAllCategoriesByAdminAsync()
        {
            return await _context.Categories
            .AsNoTracking()
            .Select(c => new GetCategorySimpleByAdminDto(c.Id,c.Name))
            .ToListAsync();
        }

        public async Task<GetCategoryWithProductsByAdminDto?> GetCategoryByAdminByIdAsync(int id)
        {

            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new GetCategoryWithProductsByAdminDto(
                    c.Name,
                    c.Products.Select(p => new GetProductToSellDto(
                        p.Provider.Code,
                        p.ProductCode,
                        p.Name,
                        p.Price
                    ))
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<GetCategorySimpleByAdminDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new InvalidOperationException($"Category not found.");
            
            await ValidateCategoryAsync(dto.Name, id);
            
            existing.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new GetCategorySimpleByAdminDto(existing.Id, existing.Name);
        }

        public async Task DeleteCategoryByAdminAsync(int id)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new InvalidOperationException($"Category not found.");
            
                _context.Categories.Remove(existing);
                await _context.SaveChangesAsync();
            
        }


        //Tasks to clients

        public async Task<IEnumerable<GetCategorySimpleByClientDto>> GetAllCategoriesByClientAsync()
        {
            return await _context.Categories
            .AsNoTracking()
            .Select(c => new GetCategorySimpleByClientDto(c.Name))
            .ToListAsync();
        }

        public async Task<GetCategoryWithProductsByClientDto?> GetCategoryByClientByIdAsync(int id)
        {

            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new GetCategoryWithProductsByClientDto(
                    c.Name,
                    c.Products.Select(p => new GetProductToSellDto(
                        p.Provider.Code,
                        p.ProductCode,
                        p.Name,
                        p.Price
                    ))
                ))
                .FirstOrDefaultAsync();
        }


        //Internal functions

        private async Task ValidateCategoryAsync(string name, int? categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("The field 'name' cannot be empty");

            var exists = await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!categoryId.HasValue || c.Id != categoryId.Value));

            if (exists)
                throw new InvalidOperationException(
                    $"The name '{name}' is already used.");
        }
    }
}
