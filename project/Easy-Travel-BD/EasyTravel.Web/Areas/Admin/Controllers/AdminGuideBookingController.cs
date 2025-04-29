using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _adminGuideBookingService.GetPaginatedGuideBookingAsync(pageNumber,pageSize);
            return View(list);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var agency = _adminGuideBookingService.Get(id);
            return View(agency);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(GuideBooking model)
        {
            if (ModelState.IsValid)
            {
                _adminGuideBookingService.Delete(model.Id);
                TempData["success"] = "The guide booking was deleted";

                return RedirectToAction("Index", "AdminGuideBooking", new { area = "Admin" });
            }
            return View(model);
        }
    }
}
