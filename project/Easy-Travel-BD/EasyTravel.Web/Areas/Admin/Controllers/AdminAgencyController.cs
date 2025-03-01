using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAgencyController : Controller
    {
        private readonly IAdminAgencyService _agencyService;
        public AdminAgencyController(IAdminAgencyService agencyService)
        {
            _agencyService = agencyService;
        }
        public IActionResult Index()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            var agencies = _agencyService.GetAll();
            return View(agencies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Agency model)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (ModelState.IsValid)
            {
                _agencyService.Create(model);
                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
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
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (ModelState.IsValid)
            {
                _agencyService.Update(model);
                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Agency model)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _agencyService.Delete(model.Id);
            return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
        }
    }
}