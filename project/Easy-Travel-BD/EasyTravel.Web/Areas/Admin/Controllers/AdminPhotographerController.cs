using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin,photographerManager")]
    public class AdminPhotographerController : Controller
    {
        private readonly IAdminPhotographerService _adminPhotographerService;
        private readonly IAgencyService _agencyService;

        public AdminPhotographerController(IAdminPhotographerService adminPhotographerService, IAgencyService agencyService)
        {
            _adminPhotographerService = adminPhotographerService;
            _agencyService = agencyService;
        }
        public IActionResult Index()
        {
            
            var photographers = _adminPhotographerService.GetAll();
            return View(photographers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = _adminPhotographerService.GetPhotographerInstance();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Photographer model)
        {
            
            if (ModelState.IsValid)
            {
                _adminPhotographerService.Create(model);
                return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
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
            var model = _adminPhotographerService.Get(id);
            model.Agencies = _agencyService.GetAll().ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Photographer model)
        {
            
            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _adminPhotographerService.Update(model);
                return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
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
            var model = _adminPhotographerService.Get(id);
            model.Agencies = _agencyService.GetAll().ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Photographer model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _adminPhotographerService.Delete(model.Id);
            return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
        }
    }
}