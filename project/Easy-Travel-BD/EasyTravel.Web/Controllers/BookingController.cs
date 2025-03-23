using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class BookingController : Controller
    {

        public BookingController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
