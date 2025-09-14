using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Trip
{
    public class TripCreateDTO
    {
        [Required, MaxLength(160)]
        public string Title { get; set; } = "";

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Required] public DateTime DateFromUtc { get; set; }
        [Required] public DateTime DateToUtc { get; set; }

        [Required] public Guid LocationId { get; set; }
        [Range(1, 1000)] public int MaxParticipants { get; set; } = 10;
        [Range(0, 9999999999.99)] public decimal? Budget { get; set; }
    }
}
