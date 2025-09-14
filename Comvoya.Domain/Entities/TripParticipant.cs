using Comvoya.Domain.Enums;
using ComvoyaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Entities
{
    public class TripParticipant
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }

        public TripParticipantRole Role { get; set; }
        public TripParticipantStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
        public string? Note { get; set; }

        public Trip Trip { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
