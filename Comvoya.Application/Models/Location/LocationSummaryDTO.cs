using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Location
{
    public class LocationSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Admin1 { get; set; }
        public string? Admin2 { get; set; }
        public string CountryIso2 { get; set; } = null!;

        public LocationSummaryDTO() { }

        public LocationSummaryDTO(Guid id, string name, string? admin1, string? admin2, string countryIso2)
        {
            Id = id;
            Name = name;
            Admin1 = admin1;
            Admin2 = admin2;
            CountryIso2 = countryIso2;
        }
    }
}