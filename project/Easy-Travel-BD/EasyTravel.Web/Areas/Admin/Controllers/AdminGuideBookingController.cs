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
        public IActionResult Index()
        {
            var list = _adminGuideBookingService.GetAll();
            return View(list);
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {

            if (id == Guid.Empty)
            {
                TempData["error"] = "The guide booking not found";
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _adminGuideBookingService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(GuideBooking model)
        {

            if (model.Id == Guid.Empty)
            {
                TempData["error"] = "The guide booking  not found";

                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _adminGuideBookingService.Delete(model.Id);
            TempData["success"] = "The guide booking was deleted";

            return RedirectToAction("Index", "AdminGuideBooking", new { area = "Admin" });
        }
    }
}
