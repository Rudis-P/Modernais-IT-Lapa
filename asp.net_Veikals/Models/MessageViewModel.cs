using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
