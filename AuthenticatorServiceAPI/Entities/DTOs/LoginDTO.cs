namespace AuthenticatorServiceAPI.Entities.DTOs
{
    public record LoginDTO(string Email, string Password);

    public record AuthResponseDTO(
        string Token,
        DateTime ExpiresAt,
        string Role
    );
}
