using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class GuideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
