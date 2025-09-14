using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string CountryIso2 { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Admin1 { get; set; }
        public string? Admin2 { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Timezone { get; set; }
        public string? Provider { get; set; }
        public string? ProviderPlaceId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public Country Country { get; set; } = null!;

    }
}
