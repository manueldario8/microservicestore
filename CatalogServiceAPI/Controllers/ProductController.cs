using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /*Endpoints to be used by admins*/
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            if (dto == null)
                return BadRequest();

            try
            {
                var created = await _productService.CreateProductAsync(dto);
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

        [HttpGet("{id:int}")]
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

        [HttpPut("{id:int}")]
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
        [HttpPut("toggle/{id:int}")]
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

        [HttpDelete("{id:int}")]
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
