using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Profile.Controllers
{
    [Area("Profile"),Authorize]
    public class BookingHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Guide()
        {
            return View();
        }
    }
}
