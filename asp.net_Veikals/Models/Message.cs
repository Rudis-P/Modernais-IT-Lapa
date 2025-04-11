using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace asp.net_Veikals.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public string? UserId { get; set; } // Nullable for guests

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
