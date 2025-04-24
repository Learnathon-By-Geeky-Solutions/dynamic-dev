using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
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
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            if (id == Guid.Empty)
            {
                TempData["error"] = "The photographer booking not found";
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _adminPhotographerBookingService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(PhotographerBooking model)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            if (model.Id == Guid.Empty)
            {
                TempData["error"] = "The photographer booking not found";

                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _adminPhotographerBookingService.Delete(model.Id);
            TempData["success"] = "The photographer booking was deleted";

            return RedirectToAction("Index", "AdminPhotographerBooking", new { area = "Admin" });
        }
    }
}
