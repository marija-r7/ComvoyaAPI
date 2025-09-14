using Comvoya.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.TripParticipant
{
    public class TripParticipantAddDTO
    {
        public Guid? UserId { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
        public string? InviteCode { get; set; }
        public string? Note { get; set; }
    }
}
