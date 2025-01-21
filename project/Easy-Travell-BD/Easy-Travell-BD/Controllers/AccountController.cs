using Microsoft.AspNetCore.Mvc;

namespace Easy_Travell_BD.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
