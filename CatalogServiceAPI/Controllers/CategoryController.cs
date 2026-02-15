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
            if (dto == null)
                return BadRequest();

            try
            {
                var created = await _categoryService.CreateCategoryAsync(dto);

                return CreatedAtAction(nameof(GetCategoryByIdAdmin), new { id = created.Id }, created);

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

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

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPut("adm/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (dto == null)
                return BadRequest();

            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, dto);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("adm/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryByAdminAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
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

            if (category == null)
                return NotFound();

            return Ok(category);
        }
    }
}
