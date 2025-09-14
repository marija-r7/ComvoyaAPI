using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Application.Models.User
{
    public class UserChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required, MinLength(8)]
        public string NewPassword { get; set; } = "";
        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }
    }
}
