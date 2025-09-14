using Comvoya.Application.Models.Interest;

namespace Comvoya.Application.Models.User
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public List<InterestDTO>? Interests { get; set; }
    }
}
