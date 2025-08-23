using ComvoyaAPI.Domain.Entities;

namespace ComvoyaAPI.Application.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public List<InterestsDTO> Interests { get; set; }
    }
}
