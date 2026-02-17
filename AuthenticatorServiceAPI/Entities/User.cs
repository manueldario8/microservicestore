namespace AuthenticatorServiceAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string   Email {  get; set; }
        public string PasswordHash { get; set; } = default!;
        public Roles Role { get; set; } = Roles.Client;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
