using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace SmartAPIManager.Web.Controllers
{
    public class ProjectJsonController : Controller
    {
        private readonly ProjectJsonService _projectJsonService;
        public ProjectJsonController(ProjectJsonService projectJsonService)
        {
            _projectJsonService = projectJsonService;
            
        }

        //Get:ProjectJson

        public async Task<IActionResult> Index()
        {
            var projectJsons = await _projectJsonService.GetAllAsync();
            return View(projectJsons);
        }

        //Get:Create
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectJson projectJson)
        {
            if (ModelState.IsValid)
            {
                await _projectJsonService.SaveAsync(projectJson);
                return RedirectToAction("Index");
            }
            return View(projectJson);
        }

        //Get:Edit

        public async Task<IActionResult> Edit(int id)
        {
            var projectJson = await _projectJsonService.GetByIdAsync(id);
            if (projectJson == null) 
            {
                return NotFound();  
            }
            return View(projectJson);
        }

        //Post:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, ProjectJson projectJson)
        {
            if(id != projectJson.Id)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            {
                await _projectJsonService.UpdateAsync(projectJson);
                return RedirectToAction(nameof(Index));
            }
            return View(projectJson);

        }

        //Get:Delete
        public async Task<IActionResult> Delete(int id)
        {
            var projectJsons = await _projectJsonService.GetByIdAsync(id);
            if(projectJsons == null)
            {
                return NotFound();
            }
            return View(projectJsons);
        }

        //Post:Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            await _projectJsonService.DeleteAsync(x => x.Id == id);
            return RedirectToAction(nameof(Index));
        }



    }
}
