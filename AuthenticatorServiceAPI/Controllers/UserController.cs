using AuthenticatorServiceAPI.Entities.DTOs;
using AuthenticatorServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticatorServiceAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<ActionResult<CreatedUserDTO>> Create(CreateUserDTO dto)
        {
            return Ok(await _userService.CreateUser(dto));
        }
    }
}
