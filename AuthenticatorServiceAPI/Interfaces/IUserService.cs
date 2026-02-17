using AuthenticatorServiceAPI.Entities;
using AuthenticatorServiceAPI.Entities.DTOs;

namespace AuthenticatorServiceAPI.Interfaces
{
    public interface IUserService
    {
        Task<CreatedUserDTO> CreateUser(CreateUserDTO dto);
        Task<User?> GetByEmailAsync(string email);
    }
}
