using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Helpers
{
    public sealed class LocationQuery
    {
        public string? Query { get; init; }
        public string? Admin1 { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
