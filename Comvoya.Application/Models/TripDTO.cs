using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models
{
    public class TripDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? Budget { get; set; }
        public int? MaxParticipants { get; set; }
    }
}
