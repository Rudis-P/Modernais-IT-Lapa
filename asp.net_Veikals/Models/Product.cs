using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace asp.net_Veikals.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc{ get; set; }
        public string LongDesc { get; set; }
        public decimal Price { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public Category Category { get; set; }
        public DateTime UpdatedAt {  get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
        public string Slug { get; set; }
    }
}
