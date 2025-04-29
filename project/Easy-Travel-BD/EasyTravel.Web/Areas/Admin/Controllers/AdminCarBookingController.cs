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
        public async Task<IActionResult> Index(int pageNumber = 1,int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _adminCarBookingService.GetPaginatedCarBookingsAsync(pageNumber,pageSize);
            return View(list);
        }
    }
}
