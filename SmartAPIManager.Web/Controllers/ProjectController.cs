using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;
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
                var project = await _projectService.GetByIdAsync(id.Value);
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
            // Kullanıcının e-posta adresini alıyoruz
            var email = User.Identity.Name;
            var user = await _userService.SelectAsync(u => u.Email == email);

            if (user == null)
            {
                // Kullanıcı bilgisi alınamazsa hata ekliyoruz
                ModelState.AddModelError("", "Kullanıcı bilgisi alınamadı.");
                return View(model);
            }

            // Mevcut proje mi güncelleniyor yoksa yeni proje mi oluşturuluyor, kontrol ediyoruz
            var project = model.ProjectId > 0
                ? await _projectService.GetByIdAsync(model.ProjectId)
                : new Project();

            if (project == null)
            {
                return NotFound();
            }

            // Kullanıcıyı proje ile ilişkilendiriyoruz
            project.User = user;
            _projectService.Attach(user);

            if (ModelState.IsValid)
            {
                // Proje bilgilerini güncelliyoruz
                project.Name = model.Name;
                project.Description = model.Description;
                project.UploadDate = model.UploadDate;

                // Mevcut proje dosyalarını alıyoruz
                var existingProject = await _projectService.GetByIdAsync(model.ProjectId);
                project.ProjectFile = existingProject?.ProjectFile ?? new List<ProjectFile>();

                // Dosya yükleme işlemi
                if (model.ProjectFile != null && model.ProjectFile.Any())
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var formFile in model.ProjectFile)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.Combine(uploadsFolder, formFile.FileName);

                            // Dosyayı yüklüyoruz
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            var projectFile = new ProjectFile
                            {
                                FileName = formFile.FileName,
                                FileWay = $"/uploads/{formFile.FileName}",
                                UploadDate = DateTime.Now
                            };

                            // Yeni dosyayı mevcut ProjectFile listesine ekliyoruz
                            project.ProjectFile.Add(projectFile);
                        }
                    }
                }
                else
                {
                    // Eğer dosya eklenmiyorsa mevcut dosyaların korunmasını sağlıyoruz
                    project.ProjectFile = existingProject?.ProjectFile ?? new List<ProjectFile>();
                }

                // Proje kaydetme işlemi: Yeni proje mi yoksa mevcut proje mi?
                if (model.ProjectId == 0)
                {
                    // Yeni proje ekleme işlemi
                    await _projectService.SaveAsync(project);
                }
                else
                {
                    // Mevcut projeyi güncelleme işlemi
                    await _projectService.UpdateAsync(project);
                }

                // Değişiklikleri veritabanına kaydediyoruz
                await _projectService.SaveChangesAsync(); // Bu önemli, son değişiklikleri mutlaka kaydediyoruz.

                return RedirectToAction(nameof(Index));
            }

            // ModelState geçerli değilse formu yeniden yüklüyoruz
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
            await _projectService.DeleteAsync(x => x.ProjectId == id);
            return RedirectToAction(nameof(Index));
        }






    }
}
