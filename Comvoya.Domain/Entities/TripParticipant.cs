using Comvoya.Domain.Enums;
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
    }
}
