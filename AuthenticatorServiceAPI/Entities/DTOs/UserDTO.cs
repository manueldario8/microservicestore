namespace AuthenticatorServiceAPI.Entities.DTOs
{
    public record CreateUserDTO(
        string Email,
        string Password);

    public record CreatedUserDTO(
        Guid Id,
        string Email,
        Roles Role,
        bool IsActive,
        DateTime DateTime);
}
