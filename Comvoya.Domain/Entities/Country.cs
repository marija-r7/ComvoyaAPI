using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Entities
{
    public class Country
    {
        public string Iso2 { get; set; } = default!;
        public string Iso3 { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Emoji { get; set; }
        public ICollection<Location> Locations { get; set; } = [];
    }
}
