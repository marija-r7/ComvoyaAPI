using Comvoya.Application.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Comvoya.Application.Models.Interest
{
    public class InterestDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public required string Name { get; set; } = "";
    }
}
