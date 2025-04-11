using System.Security.Claims;
using asp.net_Veikals.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.net_Veikals.Controllers
{
    public class ApplicationUser : IdentityUser
    {
    }

    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // Login Action
        [HttpPost]
        public async Task<IActionResult> Login(LoginSignupViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("Login State 1");
            ModelState.Remove(nameof(model.SignupModel));
            if (model.LoginModel.Email == "admin")
            {
                ModelState.Remove(nameof(model.LoginModel.Email));  
            }
            System.Diagnostics.Debug.WriteLine($"Debug ModelState.IsValid: {ModelState.IsValid}");
            System.Diagnostics.Debug.WriteLine($"LoginModel Email: {model.LoginModel.Email}");
            System.Diagnostics.Debug.WriteLine($"LoginModel Password: {model.LoginModel.Password}");
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                System.Diagnostics.Debug.WriteLine("Login State 2");
                return View("~/Views/Home/Account/acc.cshtml", model);
            }
            System.Diagnostics.Debug.WriteLine("Login State 3");

            var result = await _signInManager.PasswordSignInAsync(model.LoginModel.Email, model.LoginModel.Password, true, false);
                System.Diagnostics.Debug.WriteLine($"SignInResult: {result}");

            if (result.Succeeded)
            {
                System.Diagnostics.Debug.WriteLine("Login State 5");

                var user = await _userManager.FindByEmailAsync(model.LoginModel.Email);
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine("Login State 6");
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name ?? "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Identity.Application");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    System.Diagnostics.Debug.WriteLine("Login State 7");
                    return RedirectToAction("UserAccount", "Account", new { id = user.Id });
                }
                //await _signInManager.SignInAsync(user, isPersistent: false);
                System.Diagnostics.Debug.WriteLine("Login State 4");
                //return RedirectToAction("UserAccount", new { id = user.Id });
            }


            System.Diagnostics.Debug.WriteLine("", "Invalid login attempt.");
            return View("~/Views/Home/Account/acc.cshtml", model);
        }


        // Signup Action
        [HttpPost]
        public async Task<IActionResult> Signup(LoginSignupViewModel model)
        {

            System.Diagnostics.Debug.WriteLine("Debug Signup method called.");
            System.Diagnostics.Debug.WriteLine($"Debug ModelState.IsValid: {ModelState.IsValid}");


            System.Diagnostics.Debug.WriteLine("State 1");

            ModelState.Remove(nameof(model.LoginModel));

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                System.Diagnostics.Debug.WriteLine("State 2");
                return View("~/Views/Home/Account/acc.cshtml", model);
            }

            var user = new User { UserName = model.SignupModel.Email, Email = model.SignupModel.Email, Name = model.SignupModel.Email};
            var result = await _userManager.CreateAsync(user, model.SignupModel.Password);
            System.Diagnostics.Debug.WriteLine("State 3");
            
            System.Diagnostics.Debug.WriteLine($"Debug result.Succeeded: {result.Succeeded}");

            if (result.Succeeded)
            {
                System.Diagnostics.Debug.WriteLine("State 4");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                System.Diagnostics.Debug.WriteLine("State 5");
                ModelState.AddModelError("", error.Description);
                System.Diagnostics.Debug.WriteLine($"Field:, Error: {error.Description}");

            }
            System.Diagnostics.Debug.WriteLine("State 6");
            return View("~/Views/Home/Account/acc.cshtml", model);
        }

        // Logout Action
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> UserAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID format."); 
            }
            System.Diagnostics.Debug.WriteLine("Login State 8");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("Login State 9");
                return RedirectToAction("acc", "Account"); 
            }
            System.Diagnostics.Debug.WriteLine($"LoginModel Email: {ViewBag.UserName = user.Name}");
            ViewBag.UserName = user.Name;

            ViewData["IsAdmin"] = user.IsAdmin;

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

            if (user.IsAdmin)
            {
                return View("~/Views/Home/Account/admin_acc.cshtml", productViewModels);
            }
            else
            {
                return View("~/Views/Home/Account/user_acc.cshtml");
            }
        }


    }
}