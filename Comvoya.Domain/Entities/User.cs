using Comvoya.Domain.Entities;
using Comvoya.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ComvoyaAPI.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Lastname { get; set; }
        public required string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public required string Username { get; set; }
        public required UserRole Role { get; set; }

        public List<Interest>? Interest { get; set; }

        public void setPasswordHash(string passwordHash)
        {
            this.PasswordHash = passwordHash;
        }
    }
}
