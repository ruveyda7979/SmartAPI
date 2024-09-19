  using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SmartAPIManager.Web.Models;

namespace SmartAPIManager.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;

        // Constructor
        public ProjectController(ProjectService projectService, UserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            return View(projects);
        }


        //New Action: Projeye tıklandığında JSON sayfasına yönlendirme
        public IActionResult GoToProjectJson(int projectId)
        {
            return RedirectToAction("Index","ProjectJson", new {projectId});
        }
        public IActionResult Create()
        {
            return View();
        }

        


        public async Task<IActionResult> Edit(int? id)
          {
            if (id == null || id == 0)
            {
                // Yeni bir proje eklemek için boş bir model döndürür
                return View(new ProjectEditModel
                {
                    ProjectId = 0, // Yeni bir proje olduğunda ProjectId 0 olarak ayarlanır
                    UploadDate = DateTime.Now, // Varsayılan olarak mevcut tarih ayarlanır
                    ProjectFile = new List<IFormFile>() // Boş listeyi başlattık
                });
            }
            else
            {
                // Mevcut bir projeyi düzenlemek için
                var project = await _projectService.GetByIdWithFilesAsync(id.Value);
                if (project == null)
                {
                    return NotFound(); // Proje bulunamazsa 404 döndürür
                }

                // Mevcut proje verilerini ViewModel'e aktarır
                var projectEditModel = new ProjectEditModel
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Description = project.Description,
                    ExistingFiles = project.ProjectFile.ToList(),
                    UploadDate = project.UploadDate
                    // Gerekirse diğer alanları da burada set edebilirsiniz.
                };
                return View(projectEditModel);
            }

            //var project = await _projectService.GetByIdAsync(id) ?? new Project() { ProjectId = 0, UploadDate = DateTime.Now };
            //if (project == null)
            //{
            //    return NotFound();
            //}
            //return View(project);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectEditModel model)
        {
            var email = User.Identity.Name;
            var user = await _userService.SelectAsync(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bilgisi alınamadı.");
                return View(model);
            }

            // Proje güncelleme veya yeni proje oluşturma
            var project = model.ProjectId > 0
                ? await _projectService.GetByIdWithFilesAsync(model.ProjectId)
                : new Project();

            if (project == null)
            {
                return NotFound();
            }

            project.User = user;
            _projectService.Attach(user);

            if (ModelState.IsValid)
            {
                // Proje bilgilerini güncelle
                project.Name = model.Name;
                project.Description = model.Description;
                project.UploadDate = model.UploadDate;

                // Silinecek dosyaları işleme alıyoruz
                if (model.FilesToDelete != null && model.FilesToDelete.Any())
                {
                    var filesToRemove = project.ProjectFile
                        .Where(f => model.FilesToDelete.Contains(f.ProjectFileId)) // File ID ile seçiyoruz
                        .ToList();

                    foreach (var fileToDelete in filesToRemove)
                    {
                        // Fiziksel dosyayı siliyoruz
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + fileToDelete.FileWay);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath); // Fiziksel dosyayı sil
                        }

                        // Veritabanından dosyayı sil
                        await _projectService.RemoveFileAsync(fileToDelete); // Dosyayı veritabanından sil

                    }

                    // Değişiklikleri veritabanında kaydediyoruz
                    await _projectService.SaveChangesAsync();
                }

                // Yeni dosyalar eklenmişse onları yükleyelim
                if (model.ProjectFile != null && model.ProjectFile.Any())
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var formFile in model.ProjectFile)
                    {
                        if (formFile.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            // Dosyayı fiziksel olarak yükle
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Veritabanına yeni dosya kaydını ekle
                            var projectFile = new ProjectFile
                            {
                                FileName = uniqueFileName,
                                FileWay = $"/uploads/{uniqueFileName}",
                                UploadDate = DateTime.Now,
                                Project = project
                            };

                            project.ProjectFile.Add(projectFile);
                        }
                    }
                }

                // Proje güncelleme veya yeni proje ekleme işlemi
                if (model.ProjectId == 0)
                {
                    await _projectService.SaveAsync(project);
                }
                else
                {
                    await _projectService.UpdateAsync(project);
                }

                await _projectService.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }




        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Servis katmanındaki metodu çağırıyoruz
            await _projectService.DeleteProjectWithFilesAsync(id);
            return RedirectToAction(nameof(Index));
        }






    }
}
