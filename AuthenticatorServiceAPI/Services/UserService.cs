using AuthenticatorServiceAPI.Data;
using AuthenticatorServiceAPI.Entities;
using AuthenticatorServiceAPI.Entities.DTOs;
using AuthenticatorServiceAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatorServiceAPI.Services
{
    public class UserService(AuthDbContext context) : IUserService
    {
        private readonly AuthDbContext _context = context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public async Task<CreatedUserDTO> CreateUser(CreateUserDTO dto)
        {
            var email = dto.Email.Trim().ToLower();
            await ValidateUser(email, dto.Password);

            var user = new User
            {
                Email = email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new CreatedUserDTO(user.Id, user.Email, user.Role,user.IsActive, user.CreatedAt);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    

    private async Task ValidateUser(string email, string password)
    {

            if (string.IsNullOrEmpty(email)) throw new InvalidOperationException("Se requiere un email para continuar");
            if (string.IsNullOrEmpty(password)) throw new InvalidOperationException("La contraseña no puede estar vacía");
            if (password.Length < 6) throw new InvalidOperationException("La contraseña no puede tener menos de 6 caracteres");

            var emailUsed = await _context.Users.AnyAsync(p => p.Email == email);
            if (emailUsed)
                throw new InvalidOperationException($"El mail '{email}' ya está asignado a otro usuario.");
        }
    }
}
