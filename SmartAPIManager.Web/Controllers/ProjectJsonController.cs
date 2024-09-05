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

        //Save JSON (Create or Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJson(ProjectJsonViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                        await _projectJsonService.UpdateAsync(projectJson);
                    }

                }

                return RedirectToAction("Index", new {ProjectId = model.ProjectId});
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
