using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetCoreSqlDb.Helpers;
//using DotNetCoreSqlDb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    //[RequireHttps]
    public class LoginController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly LogHelper _logHelper;

        public LoginController(MyDatabaseContext context, LogHelper logHelper)
        {
            _context = context;
            _logHelper = logHelper;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string username, string password)
        {

            //ViewBag.ShowForgotPassword = true;
            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials. Please try again.";
                return View();
            }

            //bool valid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            bool primaryValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            
            //if (!valid)
            if (!primaryValid)
            {
                ViewBag.Error = "Invalid credentials. Please try again.";
                return View();
            }
            await _logHelper.LogSignInAsync(user.ID, username);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserID", user.ID.ToString())
            };

            // Create a claims identity specifying the authentication scheme
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Set up authentication properties if needed (e.g., IsPersistent for "Remember Me")
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1440)
                // IsPersistent = true
            };

            // Sign in the user (this creates an encrypted cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect based on role

            return RedirectToAction("Index", "Home");

        }

        // Simple logout action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}