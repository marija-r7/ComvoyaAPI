using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Country
{
    public class CountryDetailsDTO
    {
        public string Iso2 { get; set; } = null!;
        public string Iso3 { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Emoji { get; set; }
        public int LocationsCount { get; set; }

        public CountryDetailsDTO() { }

        public CountryDetailsDTO(string iso2, string iso3, string name, string? emoji, int locationsCount)
        {
            Iso2 = iso2;
            Iso3 = iso3;
            Name = name;
            Emoji = emoji;
            LocationsCount = locationsCount;
        }
    }

}
