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
         public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var photographers = await _adminPhotographerService.GetPaginatedPhotographersAsync(pageNumber,pageSize);
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
            if (!ModelState.IsValid)
            {
                return View();
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
            if (ModelState.IsValid)
            {
                return View();
            }
            var model = _adminPhotographerService.Get(id);
            model.Agencies = _agencyService.GetAll().ToList();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Photographer model)
        {
            if (ModelState.IsValid)
            {
                _adminPhotographerService.Delete(model.Id);
                return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
            }
            return View(model);
        }
    }
}