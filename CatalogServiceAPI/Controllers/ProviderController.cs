using CatalogServiceAPI.Entities.DTOs;
using CatalogServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController(IProviderService providerService) : ControllerBase
    {
        private readonly IProviderService _providerService = providerService;

        /*Endpoints to be used by admins*/
        [HttpPost]
        public async Task<IActionResult> CreateProvider([FromBody] CreateProviderDto dto) 
        {
            if (dto == null)
                return BadRequest();

            try
            {
                var created = await _providerService.CreateProviderAsync(dto);
                return CreatedAtAction(nameof(GetProviderById), new { id = created.Id}, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            return Ok(providers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProviderById(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProvider(int id, [FromBody] UpdateProviderDto dto)
        {
            if (dto == null)
                return BadRequest();

            try
            {
                var updated = await _providerService.UpdateProviderAsync(id, dto);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("toggle/{id:int}")]
        public async Task<IActionResult> ToggleStatusActived(int id)
        {
            try 
            {
                var toggled = await _providerService.ToggleStatusProviderAsync(id);
                return Ok(toggled);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            try
            {
                await _providerService.DeleteProviderAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
