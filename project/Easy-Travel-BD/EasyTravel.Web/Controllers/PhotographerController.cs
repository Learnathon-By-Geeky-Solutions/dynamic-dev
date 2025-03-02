using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        public PhotographerController(IPhotographerService photographerService)
        {
            _photographerService = photographerService;
        }
        public IActionResult Index()
        {
            var models = _photographerService.GetAll();
            return View(models);
        }
    }
}
