using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles = "admin")]
    public class AdminRoleController : Controller
    {
        private readonly IAdminRoleService _adminRoleService;
        public AdminRoleController(IAdminRoleService adminRoleService)
        {
            _adminRoleService = adminRoleService;
        }
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminRole/Index");
            var roles = await _adminRoleService.GetAllAsync();
            return View(roles);
        }
    }
}
