using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin")]
    public class AdminUserController : Controller
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminRoleService _adminRoleService;
        public AdminUserController(IAdminUserService adminUserService, IAdminRoleService adminRoleService)
        {
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Index");
            var roles = await _adminUserService.GetAllAsync();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Create");
            var roles = _adminRoleService.GetAllAsync();
            var user = new UserViewModel();
            user.Roles = roles.Result.ToList();
            return View(user);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (success,errormessage) = await _adminUserService.CreateAsync(model.FirstName,model.LastName,model.DateOfBirth,model.Gender,model.Email,model.Email,model.Password,model.Role);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, errormessage);
                    return View(model);
                }
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Update");
            var roles = _adminRoleService.GetAllAsync();
            var user = _adminUserService.GetAsync(id);
            var model = new UserViewModel
            {
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                DateOfBirth = user.Result.DateOfBirth,
                Gender = user.Result.Gender,
                Email = user.Result.Email,
                Roles = roles.Result.ToList()

            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _adminUserService.UpdateAsync(model.FirstName, model.LastName, model.DateOfBirth, model.Gender, model.Email, model.Email, model.Role);
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Delete");
            var model = await _adminUserService.GetAsync(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User model)
        {
            if (ModelState.IsValid)
            {
                await _adminUserService.DeleteAsync(model.Id);
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            return View(model);
        }
    }
}
