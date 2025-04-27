using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,agencyManager")]
    public class AdminAgencyController : Controller
    {
        private readonly IAdminAgencyService _agencyService;
        public AdminAgencyController(IAdminAgencyService agencyService)
        {
            _agencyService = agencyService;
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            
            var agencies = await _agencyService.GetPaginatedAgenciesAsync(pageNumber,pageSize);
            return View(agencies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Agency model)
        {
            
            if (ModelState.IsValid)
            {
                _agencyService.Create(model);
                TempData["success"] = "The agency has been created successfully";
                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Agency model)
        {
            if (ModelState.IsValid)
            {
                _agencyService.Update(model);
                TempData["success"] = "The agency has been updated successfully";

                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == Guid.Empty)
            {
                TempData["error"] = "The agency not found";
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Agency model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _agencyService.Delete(model.Id);
            TempData["success"] = "The agency was deleted";

            return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
        }
    }
}