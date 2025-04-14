using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,carManager")]
    public class AdminCarBookingController : Controller
    {
        private readonly IAdminCarBookingService _adminCarBookingService;
        public AdminCarBookingController(IAdminCarBookingService adminCarBookingService)
        {
            _adminCarBookingService = adminCarBookingService;
        }
        public IActionResult Index()
        {
            var list = _adminCarBookingService.GetAll();
            return View(list);
        }
    }
}
