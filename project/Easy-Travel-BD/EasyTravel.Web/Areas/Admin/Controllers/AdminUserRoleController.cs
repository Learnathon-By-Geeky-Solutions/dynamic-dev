using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Linq.Expressions;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles = "admin")]
    public class AdminUserRoleController : Controller
    {
        private readonly IAdminUserRoleService _adminUserRoleService;
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminRoleService _adminRoleService;
        public AdminUserRoleController(IAdminUserRoleService adminUserRoleService, IAdminUserService adminUserService, IAdminRoleService adminRoleService)
        {
            _adminUserRoleService = adminUserRoleService;
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUserRole/Index");
            var userRoles = await _adminUserRoleService.GetAllAsync();
            return View(userRoles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUserRole/Create");
            var model = new AdminUserRoleModel()
            {
                Users = await _adminUserRoleService.GetUsersWithoutRole(),
                Roles = _adminRoleService.GetAll().ToList(),
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminUserRoleService.CreateAsync(model.UserId, model.RoleId);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AdminUserRole", new { area = "Admin" });
                }
            }
            model.Users = await _adminUserRoleService.GetUsersWithoutRole();
            model.Roles = _adminRoleService.GetAll().ToList();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUserRole/Update");
            var ids = id.Split(",");
            var model = new AdminUserRoleModel()
            {
                UserId = Guid.Parse(ids[0]),
                RoleId = Guid.Parse(ids[1]),
                Users = new List<User> { await _adminUserService.GetAsync(Guid.Parse(ids[0])) },
                Roles = _adminRoleService.GetAll().ToList(),
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdminUserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminUserRoleService.UpdateAsync(model.UserId, model.RoleId);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AdminUserRole", new { area = "Admin" });
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUserRole/Delete");
            var ids = id.Split(",");
            var model = new AdminUserRoleModel()
            {
                UserId = Guid.Parse(ids[0]),
                RoleId = Guid.Parse(ids[1]),
                Users = new List<User> { await _adminUserService.GetAsync(Guid.Parse(ids[0])) },
                Roles = new List<Role>{await _adminRoleService.GetAsync(Guid.Parse(ids[1]))},
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AdminUserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminUserRoleService.DeleteAsync(model.UserId, model.RoleId);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AdminUserRole", new { area = "Admin" });
                }
            }
            model.Users = await _adminUserRoleService.GetUsersWithoutRole();
            model.Roles = _adminRoleService.GetAll().ToList();
            return View(model);
        }
    }
}
