
using Comvoya.Application.Models.UserInterest;

namespace Comvoya.Application.Models.User
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public ICollection<UserInterestDTO> UserInterests { get; set; } = new List<UserInterestDTO>();
    }
}
