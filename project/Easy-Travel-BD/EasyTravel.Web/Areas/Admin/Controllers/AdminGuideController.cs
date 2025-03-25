using EasyTravel.Application.Factories;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin,guideManager")]
    public class AdminGuideController : Controller
    {
        private readonly IAdminGuideService _guideService;
        private readonly IAgencyService _agencyService;
        public AdminGuideController(IAdminGuideService guideService, IAgencyService agencyService)
        {
            _guideService = guideService;
            _agencyService = agencyService;
        }
        public IActionResult Index()
        {
            
            var guides = _guideService.GetAll();
            return View(guides);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = _guideService.GetGuideInstance();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Guide model)
        {
            
            if (ModelState.IsValid)
            {
                _guideService.Create(model);
                return RedirectToAction("Index", "AdminGuide", new { area = "Admin" });
            }
            model.Agencies = _agencyService.GetAll().ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var model = _guideService.Get(id);
            model.Agencies = _agencyService.GetAll().ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Guide model)
        {
            
            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _guideService.Update(model);
                return RedirectToAction("Index", "AdminGuide", new { area = "Admin" });
            }
            model.Agencies = _agencyService.GetAll().ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var model = _guideService.Get(id);
            model.Agencies = _agencyService.GetAll().ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guide model)
        {
            
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _guideService.Delete(model.Id);
            return RedirectToAction("Index", "AdminGuide", new { area = "Admin" });
        }
    }
}