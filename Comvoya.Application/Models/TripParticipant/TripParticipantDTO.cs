using Comvoya.Application.Models.User;
using Comvoya.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.TripParticipant
{
    public class TripParticipantDTO
    {
        public Guid UserId { get; set; }
        public UserResponseDTO User { get; set; } = null!;
        public TripParticipantRole Role { get; set; }
        public TripParticipantStatus Status { get; set; }
    }
}
