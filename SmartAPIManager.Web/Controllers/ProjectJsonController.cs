using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SmartAPIManager.Web.Models;
using System.Linq;

namespace SmartAPIManager.Web.Controllers
{
    public class ProjectJsonController : Controller
    {
        private readonly ProjectJsonService _projectJsonService;
        public ProjectJsonController(ProjectJsonService projectJsonService)
        {
            _projectJsonService = projectJsonService;
            
        }

        //Get:ProjectJson/Index?projectId=1

        public async Task<IActionResult> Index(int projectId)
        {
            var projectJsons = await _projectJsonService.SelectManyAsync(x => x.ProjectId == projectId);
            var model = new ProjectJsonViewModel
            {
                ProjectId = projectId,
                JsonList = projectJsons?.ToList() ?? new List<ProjectJson>() //Eğer null ise boş liste döner
            };
            return View(model);
        }

        //Create
        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var model = new ProjectJsonViewModel
            {
                ProjectId = projectId,
                Date = DateTime.Now,
                JsonName = string.Empty,
                RequestURL = string.Empty,
                RelatedTable = string.Empty,
                Content = string.Empty,
                SendPattern = string.Empty,
                ReceivedPattern = string.Empty
            };
            return View("Index", model);
        }


        //Save JSON (Create or Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJson(ProjectJsonViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tarih kontrolü
                if (model.Date == DateTime.MinValue || model.Date < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                {
                    // Eğer geçersiz bir tarihse (örneğin 01.01.0001) DateTime.Now ile değiştiriyoruz.
                    model.Date = DateTime.Now;
                }

                if (model.ProjectJsonId == 0)
                {
                    var projectJson = new ProjectJson
                    {
                        ProjectId = model.ProjectId,
                        JsonName = model.JsonName,
                        UploadDate = model.Date,
                        RequestUrl = model.RequestURL,
                        RelatedTable = model.RelatedTable,
                        Content = model.Content,
                        SendPattern = model.SendPattern,
                        ReceivedPattern = model.ReceivedPattern,

                    };
                    await _projectJsonService.SaveAsync(projectJson);
                }
                else
                {
                    var projectJson = await _projectJsonService.GetByIdAsync(model.ProjectJsonId);
                    if(projectJson != null)
                    {
                        projectJson.JsonName = model.JsonName;
                        projectJson.UploadDate = model.Date;
                        projectJson.RequestUrl = model.RequestURL;
                        projectJson.RelatedTable = model.RelatedTable;
                        projectJson.Content = model.Content;
                        projectJson.SendPattern = model.SendPattern;
                        projectJson.ReceivedPattern = model.ReceivedPattern;
                        
                    }
                    await _projectJsonService.UpdateAsync(projectJson);

                }

                return RedirectToAction("Index", new {ProjectId = model.ProjectId});
            }

            if (!ModelState.IsValid)
            {
                // ModelState geçerli değilse hataları loglayalım
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını konsola yazdırın
                }

                // ModelState geçersizse formu yeniden yükleyelim
                return View("Index", model);
            }

            return View("Index",model);
        }

        //Edit JSON
        [HttpGet]
        public async Task<IActionResult>Edit(int id)
        {
            var projectJson = await _projectJsonService.GetByIdAsync(id);
            if(projectJson == null)
            {
                return NotFound();
            }

            var model = new ProjectJsonViewModel
            {
                ProjectJsonId = projectJson.ProjectJsonId,
                ProjectId = projectJson.ProjectId,
                JsonName = projectJson.JsonName,
                Date = projectJson.UploadDate,
                RequestURL = projectJson.RequestUrl,
                RelatedTable = projectJson.RelatedTable,
                Content = projectJson.Content,
                SendPattern = projectJson.SendPattern,
                ReceivedPattern = projectJson.ReceivedPattern,
            };
            return View("Index",model);
        }


        //Delete JSON
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete(int id)
        {
            var projectJson = await _projectJsonService.GetByIdAsync(id);
            if (projectJson != null)
            {
                await _projectJsonService.DeleteAsync(x => x.ProjectJsonId == id);
            }

            return RedirectToAction("Index", new { projectId = projectJson.ProjectId });
        }



    }
}
