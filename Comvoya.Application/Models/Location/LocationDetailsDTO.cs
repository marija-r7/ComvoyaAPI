using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Location
{
    public class LocationDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Admin1 { get; set; }
        public string? Admin2 { get; set; }
        public string CountryIso2 { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Timezone { get; set; }
        public string? Provider { get; set; }
        public string? ProviderPlaceId { get; set; }

        public LocationDetailsDTO() { }

        public LocationDetailsDTO(
            Guid id, string name, string? admin1, string? admin2, string countryIso2,
            decimal? latitude, decimal? longitude, string? timezone, string? provider, string? providerPlaceId)
        {
            Id = id;
            Name = name;
            Admin1 = admin1;
            Admin2 = admin2;
            CountryIso2 = countryIso2;
            Latitude = latitude;
            Longitude = longitude;
            Timezone = timezone;
            Provider = provider;
            ProviderPlaceId = providerPlaceId;
        }
    }
}