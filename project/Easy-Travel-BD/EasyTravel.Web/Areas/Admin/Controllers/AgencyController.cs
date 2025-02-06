using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}