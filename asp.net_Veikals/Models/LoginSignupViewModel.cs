using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace asp.net_Veikals.Models
{
    public class LoginSignupViewModel
    {

        public LoginViewModel LoginModel { get; set; }
        public SignupViewModel SignupModel { get; set; }
        public bool IsLoginForm { get; set; } = true; // Default to Login form

    }


}
