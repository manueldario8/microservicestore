using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Interfaces;
using CatalogServiceAPI.Services;
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

            try
            {
                var created = await _productService.CreateProductAsync(dto, urlPhoto);
                return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("adm")]
        public async Task<IActionResult> GetAllProductsByAdmins() 
        {
            return Ok(await _productService.GetAllProductsByAdminAsync());
        }

        [HttpGet("adm/{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByAdminById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("adm/{providerCode}/{productCode}")]
        public async Task<IActionResult> GetProductByCodesAdmin(string providerCode, string productCode)
        {
            var product = await _productService.GetProductByAdminByCodesAsync(providerCode, productCode);

            if (product == null)
                return NotFound();

            return Ok(product);

        }

        [HttpPut("adm/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {

            try
            {
                var updated = await _productService.UpdateProductAsync(id, dto);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("adm/toggle/{id:int}")]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {
            try
            {
                var toggled = await _productService.ToggleStatusProductAsync(id);
                return Ok(toggled);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("adm/{id:int}")]
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
        }

        /*Endpoints to be used by clients*/
        [HttpGet("cs")]
        public async Task<IActionResult> GetAllProductsByClients()
        {
            return Ok(await _productService.GetAllProductsByClientAsync());
        }
        [HttpGet("cs/{providerCode}/{productCode}")]
        public async Task<IActionResult> GetProductByClientsByCodesAdmin(string providerCode, string productCode)
        {
            var product = await _productService.GetProductByAdminByCodesAsync(providerCode, productCode);

            if (product == null)
                return NotFound();

            return Ok(product);

        }
    }
}
