using CatalogServiceAPI.Entities.DTOs;

namespace CatalogServiceAPI.Interfaces
{
    public interface ICategoryService
    {
        //To administrator
        Task<GetCategorySimpleByAdminDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<IEnumerable<GetCategorySimpleByAdminDto>> GetAllCategoriesByAdminAsync();
        Task<GetCategoryWithProductsByAdminDto?> GetCategoryByAdminByIdAsync(int id);
        Task<GetCategorySimpleByAdminDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        Task DeleteCategoryByAdminAsync(int id);

        //To clients
        Task<IEnumerable<GetCategorySimpleByClientDto>> GetAllCategoriesByClientAsync();
        Task<GetCategoryWithProductsByClientDto?> GetCategoryByClientByIdAsync(int id);
    }
}
