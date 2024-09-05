using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                // Oturum açmış kullanıcının e-posta adresini al
                var email = User.Identity.Name;

                // Kullanıcının UserId'sini al
                var user = await _userService.SelectAsync(u => u.Email == email);

                if (user != null)
                {
                    // Projeye oturum açmış kullanıcının UserId'sini ekle
                    project.UserId = user.UserId;

                    // Projeyi veritabanına kaydet
                    await _projectService.SaveAsync(project);

                    // Index sayfasına yönlendir
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Kullanıcı bulunamazsa bir hata mesajı göster
                    ModelState.AddModelError("", "Kullanıcı bilgisi alınamadı.");
                }
            }

            // ModelState geçerli değilse veya kullanıcı bulunamadıysa formu yeniden göster
            return View(project);
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
        public async Task<IActionResult> Edit( ProjectEditModel model)
        {
            // Oturum açmış kullanıcının e-posta adresini al
            var email = User.Identity.Name;
            var user = await _userService.SelectAsync(u => u.Email == email);

            if (user != null)
            {
                // UserId'yi ve User nesnesini projeye set et
                var project = model.ProjectId > 0 ? await _projectService.GetByIdAsync(model.ProjectId) : new Project();

                project.User = user;

                // User entity'sini yeniden eklemeyi önlemek için Attach metodunu kullanın
                _projectService.Attach(user);

                if (ModelState.IsValid)
                {

                    project.Name = model.Name;
                    project.Description = model.Description;
                    project.UploadDate = model.UploadDate;

                    //Eğer project.ProjectFile listesi null ise başlatılır
                    project.ProjectFile = project.ProjectFile ?? new List<ProjectFile>();


                    //ProjectFile alanını işleme 
                    if(model.ProjectFile != null && model.ProjectFile.Any())
                    {

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        foreach(var formFile in model.ProjectFile)
                        {
                            if(formFile.Length > 0)
                            {
                                var filePath = Path.Combine(uploadsFolder, formFile.FileName);

                                using(var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await formFile.CopyToAsync(stream);
                                }

                                var fileUrl = $"/uploads/{formFile.FileName}";

                                var projectFile = new ProjectFile
                                {
                                    FileName = formFile.FileName,
                                    FileWay = fileUrl,
                                    UploadDate = DateTime.Now
                                };

                                project.ProjectFile.Add(projectFile);  //ProjectFile koleksiyonuna ekle
                            }
                        }
                    }

                   
                    if (project.ProjectId == 0)
                    {
                        // Yeni proje ekleme işlemi
                        await _projectService.SaveAsync(project);
                    }
                    else
                    {
                        // Mevcut projeyi güncelleme işlemi
                        await _projectService.UpdateAsync(project);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // Kullanıcı bilgisi alınamazsa hata ekle
                ModelState.AddModelError("", "Kullanıcı bilgisi alınamadı.");
            }

            // ModelState geçerli değilse formu yeniden yükle
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
