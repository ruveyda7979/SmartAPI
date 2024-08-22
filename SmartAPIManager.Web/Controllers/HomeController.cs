using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using SmartAPIManager.Web.Models;
using SmartAPIManager.Web.Models.ViewModels;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

namespace SmartAPIManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Projects()
        {          
            return RedirectToAction("Index", "Project"); // Örnek olarak kullanýcýyý Project Index sayfasýna yönlendirme
        }

        public IActionResult Json()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ValidateUserAsync(model.Email, HashPassword(model.Password));
                if (user != null)
                {
                    return RedirectToAction("Index", "Project");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            ViewData["ErrorMessage"] = "Invalid login attempt. Please try again.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]


        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.FullName,
                    Email = model.Email,
                    Password = HashPassword(model.Password)
                };

                bool isRegistered = await _userService.RegisterUserAsync(user);
                if (isRegistered)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists.");
                }
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
