using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> AdminMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("admin_acc", "Home");
        }



    }
}
