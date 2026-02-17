using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        /*Endpoints to be used by admins*/
        [HttpPost("adm")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var created = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryByIdAdmin), new { id = created.Id }, created);
        }

        [HttpGet("adm")]
        public async Task<IActionResult> GetAllCategoriesByAdmins()
        {
            var categories = await _categoryService.GetAllCategoriesByAdminAsync();
            return Ok(categories);
        }

        [HttpGet("adm/{id:int}")]
        public async Task<IActionResult> GetCategoryByIdAdmin(int id)
        {
            var category = await _categoryService.GetCategoryByAdminByIdAsync(id);
            return Ok(category);
        }

        [HttpPut("adm/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);
            return Ok(updated);       
        }

        [HttpPut("adm/toggle/{id:int}")]
        public async Task<IActionResult> ChangeCategoryStatus(int id)
        {
            await _categoryService.ToggleCategoryByAdminAsync(id);
            return NoContent();
        }

        /*Endpoints to be used by clients*/
        [HttpGet("cs")]
        public async Task<IActionResult> GetAllCategoriesByClients()
        {
            var categories = await _categoryService.GetAllCategoriesByClientAsync();
            return Ok(categories);
        }

        [HttpGet("cs/{id:int}")]
        public async Task<IActionResult> GetCategoryByIdClient(int id)
        {
            var category = await _categoryService.GetCategoryByClientByIdAsync(id);
            return Ok(category);
        }
    }
}
