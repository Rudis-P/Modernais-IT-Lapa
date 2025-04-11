using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

    }
}
