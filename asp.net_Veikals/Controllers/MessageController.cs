using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_Veikals.Controllers
{
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public MessageController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string name, string subject, string message)
        {
            var user = await _userManager.GetUserAsync(User);

            var msg = new Message
            {
                Name = name,
                Subject = subject,
                Content = message,
                UserId = user?.Id
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            TempData["MessageSent"] = true;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
