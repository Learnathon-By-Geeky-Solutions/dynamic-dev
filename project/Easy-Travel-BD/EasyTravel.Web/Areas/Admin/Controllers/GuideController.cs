using EasyTravel.Application.Factories;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly IAgencyService _agencyService;
        private readonly IEntityFactory<Guide> _guideFactory;
        public GuideController(IGuideService guideService, IAgencyService agencyService,IEntityFactory<Guide> guideFactory)
        {
            _guideService = guideService;
            _agencyService = agencyService;
            _guideFactory = guideFactory;
        }
        public IActionResult Index()
        {
            var guides = _guideService.GetAll();
            return View(guides);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = _guideFactory.CreateInstance();
            var agencyList = _agencyService.GetAll();
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(Guide model)
        {
            if (ModelState.IsValid)
            {
                _guideService.Create(model);
                return RedirectToAction("Index", "Guide", new { area = "Admin" });
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
            var model = _guideService.Get(id);
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(Guide model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _guideService.Update(model);
                return RedirectToAction("Index", "Guide", new { area = "Admin" });
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
            var model = _guideService.Get(id);
            model.Agencies = agencyList.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(Guide model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _guideService.Delete(model.Id);
            return RedirectToAction("Index", "Guide", new { area = "Admin" });
        }
    }
}