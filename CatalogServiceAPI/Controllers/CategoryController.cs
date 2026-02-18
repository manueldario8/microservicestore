using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        /*Endpoints to be used by admins*/
        //[Authorize(Roles = "Admin")]
        [HttpPost("adm")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var created = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryByIdAdmin), new { id = created.Id }, created);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("adm")]
        public async Task<IActionResult> GetAllCategoriesByAdmins()
        {
            var categories = await _categoryService.GetAllCategoriesByAdminAsync();
            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adm/{id:int}")]
        public async Task<IActionResult> GetCategoryByIdAdmin(int id)
        {
            var category = await _categoryService.GetCategoryByAdminByIdAsync(id);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("adm/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);
            return Ok(updated);       
        }

        [Authorize(Roles = "Admin")]
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
