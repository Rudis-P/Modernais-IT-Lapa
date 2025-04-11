using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace asp.net_Veikals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult admin_acc()
        {
            return View("~/Views/Home/Account/admin_acc.cshtml");
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