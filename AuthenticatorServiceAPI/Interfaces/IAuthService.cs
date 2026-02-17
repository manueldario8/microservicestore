using AuthenticatorServiceAPI.Entities.DTOs;

namespace AuthenticatorServiceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);

    }
}
