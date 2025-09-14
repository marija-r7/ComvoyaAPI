using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.UserInterest
{
    public class UserInterestDTO
    {
        public Guid UserId { get; set; }
        public int InterestId { get; set; }
        [Required, MaxLength(30)]
        public required string Name { get; set; }
    }
}
