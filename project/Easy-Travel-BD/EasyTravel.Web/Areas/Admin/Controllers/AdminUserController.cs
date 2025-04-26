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
        private readonly IMapper _mapper;
        public AdminUserController(IAdminUserService adminUserService,IMapper mapper)
        {
            _adminUserService = adminUserService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Index");
            var roles =  _adminUserService.GetAll();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Create");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var (success,errormessage) = await _adminUserService.CreateAsync(user,model.Password!);
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
            if (!ModelState.IsValid)
            {
                //
            }
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Update");
            var user = await _adminUserService.GetAsync(id);
            var model = _mapper.Map<AdminUserViewModel>(user);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdminUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            if (ModelState.IsValid)
            {
                var user = await _adminUserService.GetAsync(model.Id);
                if(user?.Email != model.Email && await _adminUserService.IsExist(model.Email!))
                {
                    ModelState.AddModelError(string.Empty, "User already exist");
                    return View(model);
                }
                user = _mapper.Map(model,user);
                var (success, errormessage) = await _adminUserService.UpdateAsync(user!);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminUser/Delete");
            var user = await _adminUserService.GetAsync(id);
            var model = _mapper.Map<AdminUserViewModel>(user);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AdminUserViewModel model)
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
