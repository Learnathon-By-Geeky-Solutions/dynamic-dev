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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var pgBookins = await _adminPhotographerBookingService.GetPaginatedPhotographerBookingAsync(pageNumber, pageSize);
            return View(pgBookins);
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var agency = _adminPhotographerBookingService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(PhotographerBooking model)
        {
            if (ModelState.IsValid)
            {
                _adminPhotographerBookingService.Delete(model.Id);
                TempData["success"] = "The photographer booking was deleted";

                return RedirectToAction("Index", "AdminPhotographerBooking", new { area = "Admin" });
            }
            return View();
        }
    }
}
