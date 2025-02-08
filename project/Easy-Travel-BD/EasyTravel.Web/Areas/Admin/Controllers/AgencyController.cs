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
            var agencies = _agencyService.GetAllAgencies();
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
                _agencyService.AddAgency(model);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if(id == Guid.Empty)
            {
            }
            var agency = _agencyService.GetAgencyById(id);
            return View(agency);
        }
        [HttpPost]
        public IActionResult Update(Agency model)
        {
            if (ModelState.IsValid)
            {
                _agencyService.UpdateAgency(model);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                _agencyService.DeleteAgency(id);
            }
            return View();
        }
    }
}