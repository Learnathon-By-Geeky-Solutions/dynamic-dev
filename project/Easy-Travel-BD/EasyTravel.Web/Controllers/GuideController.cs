using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/Index");
            var models = _guideService.GetAll();
            return View(models);
        }
    }
}
