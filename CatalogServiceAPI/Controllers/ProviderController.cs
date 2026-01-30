using CatalogServiceAPI.Entities.Models;
using CatalogServiceAPI.Interfaces;
using CatalogServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController(IProviderService providerService) : ControllerBase
    {
        private readonly IProviderService _providerService = providerService;

        [HttpPost]
        public async Task<IActionResult> CreateProvider([FromBody] Provider provider) 
        {
            if (provider == null)
                return BadRequest();

            try
            {
                var created = await _providerService.CreateProviderAsync(provider);

                return CreatedAtAction(nameof(GetProviderById), new { id = created.Id }, created);
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
            var provider = await _providerService.GetProviderById(id);

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProvider(int id, [FromBody] Provider provider)
        {
            if (provider == null)
                return BadRequest();

            try
            {
                var updated = await _providerService.UpdateProviderAsync(id, provider.Name, provider.Code);
                return Ok(updated);
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
