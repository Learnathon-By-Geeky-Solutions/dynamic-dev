using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    public class AgencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}