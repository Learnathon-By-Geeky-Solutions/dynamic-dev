using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class PhotographerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
