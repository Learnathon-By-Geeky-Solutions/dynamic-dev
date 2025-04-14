using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,agencyManager")]
    public class AdminGuideBookingController : Controller
    {
        private readonly IAdminGuideBookingService _adminGuideBookingService;
        public AdminGuideBookingController(IAdminGuideBookingService adminGuideBookingService)
        {
            _adminGuideBookingService = adminGuideBookingService;
        }
        public IActionResult Index()
        {
            var list = _adminGuideBookingService.GetAll();
            return View(list);
        }
    }
}
