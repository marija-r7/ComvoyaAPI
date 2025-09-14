using Comvoya.Domain.Entities;
using Comvoya.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ComvoyaAPI.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [Required, MaxLength(50)]
        public required string Lastname { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        [Required, MaxLength(50)]
        public required string Username { get; set; }
        public required UserRole Role { get; set; }

        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
        public ICollection<TripParticipant> Trips { get; set; } = new List<TripParticipant>();
        public ICollection<Trip> OwnedTrips { get; set; } = new List<Trip>();

        public void setPasswordHash(string passwordHash)
        {
            this.PasswordHash = passwordHash;
        }
    }
}
