using CatalogServiceAPI.Entities.Models;

namespace CatalogServiceAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> UpdateCategoryAsync(Category category, string newName);
        //Task<bool> ToggleStatusCatogorieAsync(int id);
        Task DeleteCategoryAsync(int id);
        

    }
}
