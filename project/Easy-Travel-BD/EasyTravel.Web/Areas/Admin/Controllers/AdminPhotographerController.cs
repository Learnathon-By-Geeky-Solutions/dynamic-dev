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
        private readonly IAdminPhotographerService _photographerService;
        public AdminPhotographerController(IAdminPhotographerService photographerService)
        {
            _photographerService = photographerService;
        }
        public IActionResult Index()
        {
            
            var photographers = _photographerService.GetAll();
            return View(photographers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            var model = _photographerService.GetPhotographerInstance();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Photographer model)
        {
            
            if (ModelState.IsValid)
            {
                _photographerService.Create(model);
                return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var model = _photographerService.Get(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Photographer model)
        {
            
            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _photographerService.Update(model);
                return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var model = _photographerService.Get(id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Photographer model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _photographerService.Delete(model.Id);
            return RedirectToAction("Index", "AdminPhotographer", new { area = "Admin" });
        }
    }
}