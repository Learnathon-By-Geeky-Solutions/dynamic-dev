using AutoMapper;
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
        private readonly IMapper _mapper;
        public AdminUserController(IAdminUserService adminUserService, IAdminRoleService adminRoleService,IMapper mapper)
        {
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Index");
            var roles = await _adminUserService.GetAllAsync();
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Create");
            var roles = await _adminRoleService.GetAllAsync();
            var user = _adminUserService.GetUserInstance();
            var model = _mapper.Map<AdminUserViewModel>(user);
            model.Roles = roles.ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                user.UserName = model.Email;
                var (success,errormessage) = await _adminUserService.CreateAsync(user,model.Password,model.Role);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, errormessage);
                    return View(model);
                }
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            var roles = await _adminRoleService.GetAllAsync();
            model.Roles = roles.ToList();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Update");
            var roles = await _adminRoleService.GetAllAsync();
            var user = await _adminUserService.GetAsync(id);
            var model = _mapper.Map<AdminUserViewModel>(user);
            model.Roles = roles.ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                user.UserName = model.Email;
                var (success, errormessage) = await _adminUserService.UpdateAsync(user,model.Role);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, errormessage);
                    return View(model);
                }
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            var roles = await _adminRoleService.GetAllAsync();
            model.Roles = roles.ToList();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Delete");
            var roles = await _adminRoleService.GetAllAsync();
            var user = await _adminUserService.GetAsync(id);
            var model = _mapper.Map<AdminUserViewModel>(user);
            model.Roles = roles.ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _adminUserService.GetByEmailAsync(model.Email);
                await _adminUserService.DeleteAsync(user.Id);
                return RedirectToAction("Index", "AdminUser", new { area = "Admin" });
            }
            var roles = await _adminRoleService.GetAllAsync();
            model.Roles = roles.ToList();
            return View(model);
        }
    }
}
