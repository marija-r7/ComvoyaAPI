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
        public string Provider { get; set; } = default!;
        public string ProviderPlaceId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? CountryIso2 { get; set; }
        public Country? Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string? Timezone { get; set; }
        public string? Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
