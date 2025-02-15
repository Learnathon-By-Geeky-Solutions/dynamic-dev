using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        private readonly IEntityFactory<Photographer> _photographerFactory;
        private readonly IAgencyService _agencyService;
        public PhotographerController(IPhotographerService photographerService, IAgencyService agencyService,IEntityFactory<Photographer> photographerFactory)
        {
            _photographerService = photographerService;
            _agencyService = agencyService;
            _photographerFactory = photographerFactory;
        }
        public IActionResult Index()
        {
            var photographers = _photographerService.GetAll();
            return View(photographers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = _photographerFactory.CreateInstance();
            var agencyList = _agencyService.GetAll();
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(Photographer model)
        {
            if (ModelState.IsValid)
            {
                _photographerService.Create(model);
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
            var agencyList = _agencyService.GetAll();
            var model = _photographerService.Get(id);
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(Photographer model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _photographerService.Update(model);
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
            var agencyList = _agencyService.GetAll();
            var model = _photographerService.Get(id);
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(Photographer model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _photographerService.Delete(model.Id);
            return RedirectToAction("Index", "Photographer", new { area = "Admin" });
        }
    }
}