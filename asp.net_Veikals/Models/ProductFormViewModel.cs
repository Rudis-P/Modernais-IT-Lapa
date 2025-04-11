using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string Colors { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } // Category as a string for dropdown
        //public string MainImageUrl { get; set; }

    }
}