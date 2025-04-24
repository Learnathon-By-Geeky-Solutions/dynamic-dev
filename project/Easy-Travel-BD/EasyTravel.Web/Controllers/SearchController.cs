using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISessionService _sessionService;
        public SearchController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        [HttpGet]
        public IActionResult Photographer()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Photographer(SearchFormModel model)
        {
            if (ModelState.IsValid)
            {
                _sessionService.SetString("EventDate", model.EventDate.ToString());
                _sessionService.SetString("StartTime", model.StartTime.ToString());
                _sessionService.SetString("TimeInHour", model.TimeInHour.ToString());
                _sessionService.SetString("EndTime", model.EndTime.ToString());
                _sessionService.SetString("EventType", "");
                _sessionService.SetString("EventLocation", "");
                var lastPage = _sessionService.GetString("LastVisitedPage");
                if (lastPage.Contains("Photographer"))
                {
                    return RedirectToAction("List", "Photographer");
                }
                return RedirectToAction("List", "Guide");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Guide()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Guide(SearchFormModel model)
        {
            var guide = model;
            if (ModelState.IsValid)
            {
                _sessionService.SetString("EventDate", guide.EventDate.ToString());
                _sessionService.SetString("StartTime", guide.StartTime.ToString());
                _sessionService.SetString("TimeInHour", guide.TimeInHour.ToString());
                _sessionService.SetString("EndTime", guide.EndTime.ToString());
                _sessionService.SetString("EventType", "");
                _sessionService.SetString("EventLocation", "");
                var lastPage = _sessionService.GetString("LastVisitedPage");
                if (lastPage.Contains("Photographer"))
                {
                    return RedirectToAction("List", "Photographer");
                }
                return RedirectToAction("List", "Guide");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Bus()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Bus(BusSearchFormModel model)
        {
            if (ModelState.IsValid)
            {
                // Storing search parameters in session
                _sessionService.SetString("From", model.From!);
                _sessionService.SetString("To", model.To!);
                _sessionService.SetString("DateTime", model.DepartureTime.ToString()!);

                // Redirect to the List method of BusController
                return RedirectToAction("List", "Bus");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Car()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Car(CarSearchFormModel model)
        {
            if (ModelState.IsValid)
            {
                // Storing search parameters in session
                _sessionService.SetString("From", model.From!);
                _sessionService.SetString("To", model.To!);
                _sessionService.SetString("DateTime", model.DepartureTime.ToString());
                // Redirect to the List method of BusController
                return RedirectToAction("List", "Car");
            }
            return View();
        }
    }
}
