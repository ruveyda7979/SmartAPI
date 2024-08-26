 using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace SmartAPIManager.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;

        // Constructor
        public ProjectController(ProjectService projectService,UserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllAsync();
            return View(projects);
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


        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetByIdAsync(id)??new Project() { ProjectId=0,UploadDate=DateTime.Now};
            if (project == null) 
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Project project)
        {
            

            if (ModelState.IsValid)
            {
                await _projectService.UpdateAsync(project);
                return RedirectToAction(nameof(Index));
            }

            return View(project);
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
