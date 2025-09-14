using ComvoyaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Entities
{
    public class Trip
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        [Required, MaxLength(30)]
        public required string Title { get; set; }
        [MaxLength(256)]
        public string Description { get; set; } = "";
        public DateTime DateFromUtc { get; set; }
        public DateTime DateToUtc { get; set; }

        public User Owner { get; set; } = null!;
        public Guid OwnerId { get; set; }
        public Guid LocationId { get; set; }
        public int MaxParticipants { get; set; } = 10;
        public decimal? Budget { get; set; }

        public ICollection<TripParticipant> Participants { get; set; } = new List<TripParticipant>();
    }
}
