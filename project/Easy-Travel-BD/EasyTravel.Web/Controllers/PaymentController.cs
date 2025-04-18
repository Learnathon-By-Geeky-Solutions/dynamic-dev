using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.PaymentGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using System.Collections.Specialized;
using System.Security.Claims;

namespace EasyTravel.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IGetService<Booking, Guid> _bookingService;
        private readonly UserManager<User> _userManager;
        private readonly ISessionService _sessionService;
        private readonly IPhotographerService _photographerService;
        private readonly IGuideService _guideService;
        private readonly IBusService _busService;
        private readonly ICarService _carService;
        private readonly IHotelService _hotelService;
        private readonly ILogger _logger;
        public PaymentController(IGetService<Booking, Guid> bookingService, UserManager<User> userManager, ISessionService sessionService, IPhotographerService photographerService, IGuideService guideService, IBusService busService, ICarService carService, IHotelService hotelService, ILogger logger)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _sessionService = sessionService;
            _photographerService = photographerService;
            _guideService = guideService;
            _busService = busService;
            _carService = carService;
            _hotelService = hotelService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Pay(Guid id)
        {
            var bookingmodel = _bookingService.Get(id);
            var user = await _userManager.GetUserAsync(User);
            var baseUrl = Request.Scheme + "://" + Request.Host;

            // CREATING LIST OF POST DATA
            NameValueCollection PostData = new NameValueCollection();

            PostData.Add("total_amount", $"{bookingmodel.TotalAmount}");
            PostData.Add("tran_id", "TESTASPNET1234");
            PostData.Add("success_url", baseUrl + "/Payment/Confirmation");
            PostData.Add("fail_url", baseUrl + "/Payment/Fail");
            PostData.Add("cancel_url", baseUrl + "/Payment/Cancel");

            PostData.Add("version", "3.00");
            PostData.Add("cus_name", $"{user?.FirstName},{user?.LastName}");
            PostData.Add("cus_email", $"{user?.Email}");
            PostData.Add("cus_add1", "Mirpur");
            PostData.Add("cus_add2", "Address Line Tw");
            PostData.Add("cus_city", "Dhaka");
            PostData.Add("cus_state", "Dhaka");
            PostData.Add("cus_postcode", "1216");
            PostData.Add("cus_country", "Bangladesh");
            PostData.Add("cus_phone", $"{user?.PhoneNumber}");
            PostData.Add("cus_fax", "0171111111");
            PostData.Add("ship_name", "ABC XY");
            PostData.Add("ship_add1", "Address Line On");
            PostData.Add("ship_add2", "Address Line Tw");
            PostData.Add("ship_city", "City Nam");
            PostData.Add("ship_state", "State Nam");
            PostData.Add("ship_postcode", "Post Cod");
            PostData.Add("ship_country", "Countr");
            PostData.Add("value_a", "ref00");
            PostData.Add("value_b", "ref00");
            PostData.Add("value_c", "ref00");
            PostData.Add("value_d", "ref00");
            PostData.Add("shipping_method", "NO");
            PostData.Add("num_of_item", "1");
            PostData.Add("product_name", $"{bookingmodel.BookingTypes}");
            PostData.Add("product_profile", "general");
            PostData.Add("product_category", $"{bookingmodel.BookingTypes}");

            //we can get from email notificaton
            var storeId = "musli67128c564e147";
            var storePassword = "musli67128c564e147@ssl";
            var isSandboxMood = true;

            SSLCommerzGatewayProcessor sslcz = new SSLCommerzGatewayProcessor(storeId, storePassword, isSandboxMood);

            string response = sslcz.InitiateTransaction(PostData);

            return RedirectToAction(response, "Payment", new {id});

        }

        public IActionResult Confirmation(Guid id)
        {
            if (!(!String.IsNullOrEmpty(Request.Form["status"]) && Request.Form["status"] == "VALID"))
            {
                ViewBag.SuccessInfo = "There some error while processing your payment. Please try again.";
                return View();
            }

            if (Guid.Empty == id)
            {
                _logger.LogError($"PaymentController:Confirmation: id is empty. Redirecting to Expired action");
                return RedirectToAction("Expired", "Payment", new { id });
            }
            var bookingmodel = _bookingService.Get(id);
            var bookingId = Guid.Empty;
            if (bookingmodel == null)
            {
                var photogrpaher = _photographerService.Get(id);
                var guide = _guideService.Get(id);
                var bus = _busService.GetBusById(id);
                var car = _carService.GetCarById(id);
                var hotel = _hotelService.Get(id);
                if (photogrpaher != null)
                {
                    //bookingId = GetPhotographerBookingId(id);
                }
                else if (guide != null)
                {
                }
                else if (bus != null)
                {
                }
                else if (car != null)
                {
                }
                else if (hotel != null)
                {
                }
            }

            string TrxID = Request.Form["tran_id"];
            // AMOUNT and Currency FROM DB FOR THIS TRANSACTION
            string amount = Request.Form["total_amount"];
            string currency = "BDT";

            var storeId = "musli67128c564e147";
            var storePassword = "musli67128c564e147@ssl";

            SSLCommerzGatewayProcessor sslcz = new SSLCommerzGatewayProcessor(storeId, storePassword, true);
            var resonse = sslcz.OrderValidate(TrxID, amount, currency, Request);
            var successInfo = $"Validation Response: {resonse}";
            ViewBag.SuccessInfo = successInfo;

            return View();
        }
        public IActionResult Fail()
        {
            ViewBag.FailInfo = "There some error while processing your payment. Please try again.";
            return View();
        }
        public IActionResult Cancel()
        {
            ViewBag.CancelInfo = "Your payment has been cancel";
            return View();
        }
        public IActionResult Expired()
        {
            ViewBag.CancelInfo = "Your payment session has expired";
            return View();
        }
        public PhotographerBooking BookPhotographer(Guid id)
        {
            var booking = _bookingService.Get(id);
            if (booking != null && booking.BookingTypes == BookingTypes.Photographer)
            {
                return booking.PhotographerBooking;
            }
            return null;
        }
        public GuideBooking BookGuide(Guid id)
        {
            var booking = _bookingService.Get(id);
            if (booking != null && booking.BookingTypes == BookingTypes.Guide)
            {
                return booking.GuideBooking;
            }
            return null;
        }
        public BusBooking BookBus(Guid id)
        {
            var booking = _bookingService.Get(id);
            if (booking != null && booking.BookingTypes == BookingTypes.Bus)
            {
                return booking.BusBooking;
            }
            return null;
        }
        public CarBooking BookCar(Guid id)
        {
            var booking = _bookingService.Get(id);
            if (booking != null && booking.BookingTypes == BookingTypes.Car)
            {
                return booking.CarBooking;
            }
            return null;
        }
        public HotelBooking BookHotel(Guid id)
        {
            var booking = _bookingService.Get(id);
            if (booking != null && booking.BookingTypes == BookingTypes.Hotel)
            {
                return booking.HotelBooking;
            }
            return null;
        }
    }
}
