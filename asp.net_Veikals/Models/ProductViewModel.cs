﻿using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } // Category as a string for dropdown
        public string MainImageUrl { get; set; }
        public List<string> GalleryImages { get; set; }
    }
}
