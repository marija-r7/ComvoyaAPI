using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Trip
{
    public class TripUpdateDTO
    {
        [MaxLength(160)] public string? Title { get; set; }
        [MaxLength(4000)] public string? Description { get; set; }
        public DateTime? DateFromUtc { get; set; }
        public DateTime? DateToUtc { get; set; }
        public Guid? LocationId { get; set; }
        [Range(1, 1000)] public int? MaxParticipants { get; set; }
        [Range(0, 9999999999.99)] public decimal? Budget { get; set; }
    }
}
