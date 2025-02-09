using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        public PhotographerController(IPhotographerService photographerService)
        {
            _photographerService = photographerService;
        }
        public IActionResult Index()
        {
            var photographers = _photographerService.GetAllPhotographers();
            return View(photographers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Photographer model)
        {
            if (ModelState.IsValid)
            {
                _photographerService.AddPhotographer(model);
                return RedirectToAction("Index", "Photographer", new { area = "Admin" });
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
            var Photographer = _photographerService.GetPhotographerById(id);
            return View(Photographer);
        }
        [HttpPost]
        public IActionResult Update(Photographer model)
        {
            if (ModelState.IsValid)
            {
                _photographerService.UpdatePhotographer(model);
                return RedirectToAction("Index", "Photographer", new { area = "Admin" });
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
            var Photographer = _photographerService.GetPhotographerById(id);
            return View(Photographer);
        }
        [HttpPost]
        public IActionResult Delete(Photographer model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _photographerService.DeletePhotographer(model.Id);
            return RedirectToAction("Index", "Photographer", new { area = "Admin" });
        }
    }
}