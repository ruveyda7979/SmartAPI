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
        {
            if(ModelState.IsValid)
            {
                await _projectFileService.SaveAsync(projectFile);
                return RedirectToAction(nameof(Index));
            }
            return View(projectFile);
        }

        //Get:Edit

        public async Task<IActionResult> Edit(int id)
        {
            var projectFile = await _projectFileService.GetByIdAsync(id);
            if(projectFile == null)
            {
                return NotFound();
            }
            return View (projectFile);
        }

        //Get:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, ProjectFile projectFile)
        {
            if(id != projectFile.ProjectFileId)
            {
                return BadRequest();
            } 
            if (ModelState.IsValid)
            {
                await _projectFileService.UpdateAsync(projectFile);
                return RedirectToAction(nameof(Index));
            }
            return View(projectFile);
        }

        //Get:Delete
        public async Task<IActionResult> Delete(int id)
        {
            var projectFile = await _projectFileService.GetByIdAsync(id);
            if (projectFile == null)
            {
                return NotFound();
            }
            return View(projectFile);
        }

        //Post:Delete

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectFileService.DeleteAsync(x => x.ProjectFileId == id);
            return RedirectToAction(nameof(Index));

        }



    }
}
