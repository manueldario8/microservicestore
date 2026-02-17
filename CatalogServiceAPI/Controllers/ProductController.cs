using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Interfaces;
using CatalogServiceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, IImageService imageService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IImageService _imageService = imageService;

        /*Endpoints to be used by admins*/
        [Authorize(Roles = "Admin")]
        [HttpPost("adm")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            string? urlPhoto = null;

            if (dto.Image is not null)
            {
                using var stream = dto.Image.OpenReadStream();
                urlPhoto = await _imageService.UploadImage(
                    stream,
                    dto.Image.FileName
                );
            }
            
            var created = await _productService.CreateProductAsync(dto, urlPhoto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adm")]
        public async Task<IActionResult> GetAllProductsByAdmins() 
        {
            return Ok(await _productService.GetAllProductsByAdminAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adm/{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByAdminById(id);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adm/{providerCode}/{productCode}")]
        public async Task<IActionResult> GetProductByCodesAdmin(string providerCode, string productCode)
        {
            var product = await _productService.GetProductByAdminByCodesAsync(providerCode, productCode);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("adm/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _productService.UpdateProductAsync(id, dto);
            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("adm/toggle/{id:int}")]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {

            await _productService.ToggleStatusProductAsync(id);
            return NoContent();

        }

        /*[HttpDelete("adm/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }*/

        /*Endpoints to be used by clients*/
        [HttpGet("cs")]
        public async Task<IActionResult> GetAllProductsByClients()
        {
            return Ok(await _productService.GetAllProductsByClientAsync());
        }

        [HttpGet("cs/{providerCode}/{productCode}")]
        public async Task<IActionResult> GetProductByClientsByCodesAdmin(string providerCode, string productCode)
        {
            var product = await _productService.GetProductByClientByCodesAsync(providerCode, productCode);
            return Ok(product);

        }
    }
}
