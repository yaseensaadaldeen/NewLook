using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;
using System.Diagnostics;

namespace NEWLOOK.Controllers
{
    public class AccountController : Controller
    {
        private readonly NewLookContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(NewLookContext context)
        {
            _context = context;
            _httpContextAccessor = _httpContextAccessor;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = PasswordHelper.HashPassword(model.Pswd);

                var user = await _context.MstUsers
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Pswd == hashedPassword && u.Active == "Y");

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserLevel", user.Lvl);

                    TempData["SuccessMessage"] = "Login successful.";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = "Login failed. Invalid username or password.";
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear session
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MstUser user)
        {
            ModelState.Remove("lvl");
            ModelState.Remove("active");

            if (ModelState.IsValid)
            {
                user.Pswd = PasswordHelper.HashPassword(user.Pswd);
                user.Active = "Y";
                user.Lvl = "User";

                _context.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User registered successfully.";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Registration failed. Please check your input.";
            return View(user);
        }
    }
}
