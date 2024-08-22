using DBSmartAPIManager.DAL.Entities;
using DBSmartAPIManager.DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace SmartAPIManager.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        //Get:Details

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        //Get:Create

        public IActionResult Create()
        {
            return View();
        }

        //Post:Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(User user)
        {
            
           if(ModelState.IsValid)
            {
                await _userService.SaveAsync(user);
                return RedirectToAction(nameof(Index));
            }
           return View(user);
        }

        //Get:Edit

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user==null)
            {
                return NotFound();
                
            }
            return View(user);
        }

        //Post:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid) 
            {
                await _userService.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        //Get:Delete

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(x => x.UserId == id);
            return RedirectToAction(nameof(Index));
        }
    }
}
