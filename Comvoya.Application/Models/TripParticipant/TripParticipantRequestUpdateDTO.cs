using Comvoya.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.TripParticipant
{
    public class TripParticipantRequestUpdateDTO
    {
        public TripParticipantRole? Role { get; set; }
        public TripParticipantStatus? Status { get; set; }
    }
}
