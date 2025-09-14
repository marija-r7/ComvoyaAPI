using Comvoya.Application.Models.TripParticipant;
using Comvoya.Application.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Trip
{
    public class TripDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateFromUtc { get; set; }
        public DateTime DateToUtc { get; set; }
        public Guid LocationId { get; set; }
        public decimal? Budget { get; set; }
        public int MaxParticipants { get; set; }

        public UserSummaryDTO Owner { get; set; } = null!;
        public List<TripParticipantDTO> Participants { get; set; } = new();
    }
}
