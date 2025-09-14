using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.User
{
    public class UserRegisterDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Lastname { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(2), MaxLength(50)]
        public string Username { get; set; } = "";

        [Required, MinLength(8)]
        public string Password { get; set; } = "";

        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
