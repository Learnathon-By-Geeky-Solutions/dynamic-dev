using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin")]
    public class AdminRoleController : Controller
    {
        private readonly IAdminRoleService _adminRoleService;
        public AdminRoleController(IAdminRoleService adminRoleService)
        {
            _adminRoleService = adminRoleService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminRole/Index");
            var roles = _adminRoleService.GetAll();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminRole/Create");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            if (ModelState.IsValid)
            {
                await _adminRoleService.CreateAsync(model);
                return RedirectToAction("Index", "AdminRole", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminRole/Update");
            var model = await _adminRoleService.GetAsync(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Role model)
        {
            if (ModelState.IsValid)
            {
                await _adminRoleService.UpdateAsync(model);
                return RedirectToAction("Index", "AdminRole", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminRole/Delete");
            var model = await _adminRoleService.GetAsync(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Role model)
        {
            if (ModelState.IsValid)
            {
                await _adminRoleService.DeleteAsync(model.Id);
                return RedirectToAction("Index", "AdminRole", new { area = "Admin" });
            }
            return View(model);
        }
    }
}
