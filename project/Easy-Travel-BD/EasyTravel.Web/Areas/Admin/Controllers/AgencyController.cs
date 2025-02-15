using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgencyController : Controller
    {
        private readonly IAgencyService _agencyService;
        public AgencyController(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }
        public IActionResult Index()
        {
            var agencies = _agencyService.GetAll();
            return View(agencies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Agency model)
        {
            if (ModelState.IsValid)
            {
                _agencyService.Create(model);
                return RedirectToAction("Index", "Agency", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if(id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost]
        public IActionResult Update(Agency model)
        {
            if (ModelState.IsValid)
            {
                _agencyService.Update(model);
                return RedirectToAction("Index", "Agency", new { area = "Admin" });
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
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost]
        public IActionResult Delete(Agency model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _agencyService.Delete(model.Id);
            return RedirectToAction("Index", "Agency", new { area = "Admin" });
        }
    }
}