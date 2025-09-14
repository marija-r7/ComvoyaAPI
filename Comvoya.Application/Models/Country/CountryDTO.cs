using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.Country
{
    public class CountryDTO
    {
        public string Iso2 { get; set; } = null!;
        public string Iso3 { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Emoji { get; set; }

        public CountryDTO() { }

        public CountryDTO(string iso2, string iso3, string name, string? emoji)
        {
            Iso2 = iso2;
            Iso3 = iso3;
            Name = name;
            Emoji = emoji;
        }
    }
}
