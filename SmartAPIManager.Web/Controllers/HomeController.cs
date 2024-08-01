using Microsoft.AspNetCore.Mvc;
using SmartAPIManager.Web.Models;
using System.Diagnostics;

namespace SmartAPIManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Projects()
        {
            return View();
        }

        public IActionResult Json(string project)
        {
            // 'project' parametresine göre ilgili verileri yükle ve view'a gönder
            ViewBag.ProjectName = project;
            // ViewModel veya diðer verileri burada hazýrlayabilirsiniz.
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
