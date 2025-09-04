namespace ComvoyaAPI.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public List<Interest>? Interest { get; set; }
    }
}
