using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class ProductDetailsViewModel
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongtDesc { get; set; }
        public decimal Price { get; set; }
        public List<ImageViewModel> Images { get; set; }
        public string CategoryName { get; set; }
    }

    public class ImageViewModel
    {
        public string Url { get; set; }
        public bool IsMainImage { get; set; }
    }
}
