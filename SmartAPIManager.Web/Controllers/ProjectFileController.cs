using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace SmartAPIManager.Web.Controllers
{
    public class ProjectFileController : Controller
    {
        private readonly ProjectFileService _projectFileService;

        //Constructor
        public ProjectFileController(ProjectFileService projectFileService)
        {
            _projectFileService = projectFileService;
        }

        public async Task<IActionResult> Index()
        {
            var projectFiles = await _projectFileService.GetAllAsync();
            return View(projectFiles);
        }


        //Get: ProjectFile/Ceate
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectFile projectFile)
    }
}
