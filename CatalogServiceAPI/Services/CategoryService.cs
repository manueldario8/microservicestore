using CatalogServiceAPI.Data;
using CatalogServiceAPI.DomainExceptions;
using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            return new GetCategorySimpleByAdminDto(category.Id, category.Name, category.StatusActived);
        } 

        public async Task<IEnumerable<GetCategorySimpleByAdminDto>> GetAllCategoriesByAdminAsync()
        {
            return await _context.Categories
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Select(c => new GetCategorySimpleByAdminDto(c.Id,c.Name,c.StatusActived))
            .ToListAsync();
        }

        public async Task<GetCategoryWithProductsByAdminDto?> GetCategoryByAdminByIdAsync(int id)
        {
            
            var category = await _context.Categories
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
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync()??throw new NotFoundException("La categoria no existe");

            return category;
        }

        public async Task<GetCategorySimpleByAdminDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new NotFoundException("No se encontró una categoría con ese ID");
            
            await ValidateCategoryAsync(dto.Name, id);
            
            existing.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new GetCategorySimpleByAdminDto(existing.Id, existing.Name, existing.StatusActived);
        }

        public async Task ToggleCategoryByAdminAsync(int id)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new NotFoundException("No se encontró la categoría con ese ID");
            existing.StatusActived = !existing.StatusActived;

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
            var category = await _context.Categories
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
                .FirstOrDefaultAsync()??throw new NotFoundException("La categoria no existe");
            return category;
        }

        //Internal functions
        private async Task ValidateCategoryAsync(string name, int? categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessException("El nombre no puede estar vacío");

            var exists = await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!categoryId.HasValue || c.Id != categoryId.Value));

            if (exists)
                throw new BusinessException(
                    $"La categoría '{name}' ya existe.");
        }
    }
}
