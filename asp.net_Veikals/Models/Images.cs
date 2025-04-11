using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace asp.net_Veikals.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMainImage { get; set; } // Indicates if it's the main image

        // Foreign key
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
