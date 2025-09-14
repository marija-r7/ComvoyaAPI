using Comvoya.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.TripParticipant
{
    public class TripParticipantRespondToInviteDTO
    {
        [Required] public TripParticipantStatus Decision { get; set; }
    }
}
