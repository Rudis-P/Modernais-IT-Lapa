using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace asp.net_Veikals.Models
{
    public class SignupViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
