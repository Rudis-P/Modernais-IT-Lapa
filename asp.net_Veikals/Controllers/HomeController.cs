using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace asp.net_Veikals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult index()
        {
            return View();
        }

        public IActionResult acc()
        {
            return View("~/Views/Home/Account/acc.cshtml");
        }

        public IActionResult user_acc()
        {
            return View("~/Views/Home/Account/user_acc.cshtml");
        }

        public async Task<IActionResult> admin_acc()
        {
            var products = _context.Products
            .Include(p => p.Images)
            .ToList();

            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category.ToString(),
                ShortDesc = product.ShortDesc.ToString(),
                MainImageUrl = product.Images != null && product.Images.Any()
                                ? product.Images.FirstOrDefault(img => img.IsMainImage)?.Url ?? "/images/default-image.jpg"
                                : "/images/default-image.jpg"
            }).ToList();

            var messages = await _context.Messages.ToListAsync();

            var messageViewModels = messages.Select(m => new MessageViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Subject = m.Subject,
                Content = m.Content
            }).ToList();

            var viewModel = new AdminDashboardViewModel
            {
                Products = productViewModels,
                Messages = messageViewModels
            };
            return View("~/Views/Home/Account/admin_acc.cshtml", viewModel);
        }

        public IActionResult AddProduct()
        {
            return View("~/Views/Home/Product/add_product.cshtml");
        }

        public IActionResult builder()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        public IActionResult product_details()
        {
            return View("~/Views/Home/Product/product_details");
        }

        public IActionResult Shop()
        {
            return View("~/Views/Home/Product/s_categ.cshtml");
        }

        public IActionResult support()
        {
            return View();
        }

        public IActionResult homework()
        {
            return View();
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}