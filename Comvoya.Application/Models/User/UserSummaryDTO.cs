using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.User
{
    public class UserSummaryDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        public string Name { get; set; } = "";
        public string Lastname { get; set; } = "";
    }
}
