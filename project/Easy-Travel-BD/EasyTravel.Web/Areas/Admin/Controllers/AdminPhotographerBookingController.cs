using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,agencyManager")]
    public class AdminPhotographerBookingController : Controller
    {
        private readonly IAdminPhotographerBookingService _adminPhotographerBookingService;
        public AdminPhotographerBookingController(IAdminPhotographerBookingService adminPhotographerBookingService)
        {
            _adminPhotographerBookingService = adminPhotographerBookingService;
        }
        public IActionResult Index()
        {
            var list = _adminPhotographerBookingService.GetAll();
            return View(list);
        }
    }
}
