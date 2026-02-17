using AuthenticatorServiceAPI.Entities.DTOs;
using AuthenticatorServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticatorServiceAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            return Ok(await _authService.LoginAsync(dto));
        }
    }
}
