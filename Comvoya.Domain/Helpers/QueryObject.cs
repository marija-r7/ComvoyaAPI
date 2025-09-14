using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Domain.Helpers
{
    public class QueryObject
    {
        public string? Title { get; set; } = null;
        public DateTime? DateFromUtc { get; set; }
        public DateTime? DateToUtc { get; set; }
        public decimal? Budget { get; set; } = null;
        public Guid? LocationId { get; set; }
        public Guid? OwnerId { get; set; }
        public string? Sort { get; init; } = "-createdat";
    }
}
