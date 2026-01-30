using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> UpdateCategoryAsync(int id, string newName);
        //Task<bool> ToggleStatusCatogorieAsync(int id);
        Task DeleteCategoryAsync(int id);
        

    }
}
