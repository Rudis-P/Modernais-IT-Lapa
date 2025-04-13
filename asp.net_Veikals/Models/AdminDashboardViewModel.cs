using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Models
{
    public class AdminDashboardViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
