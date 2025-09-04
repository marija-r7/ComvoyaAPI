using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComvoyaAPI.Domain.Entities
{
    public class UserInterest
    {
        public Guid UserId { get; set; }
        public int InterestId { get; set; }
        public string? Name { get; set; }
    }
}
