using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
     public  HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            //if (HttpContext.Session.GetString("UserRole") != "Admin")
            //{
            //    return RedirectToAction("Index");
            //}

            return View();
        }

        [HttpPost]
        public IActionResult Create(Hotel hotel)
        {
            //if (HttpContext.Session.GetString("UserRole") != "Admin")
            //{
            //    return RedirectToAction("Index");
            //}

            if (ModelState.IsValid)
            {
                _hotelService.CreateHotel(hotel);
                return RedirectToAction("Index");

            }

            // Log or display validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                // Log the error (you can use a logging framework or simply output to console)
                Console.WriteLine(error.ErrorMessage);
            }

            return View(hotel);
        }

      

    }
}
