using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.PaymentGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using System.Collections.Specialized;
using System.Globalization;
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
        private readonly IPaymentBookingService<PhotographerBooking, Booking, Guid> _photographerPaymentService;
        private readonly IPaymentBookingService<GuideBooking, Booking, Guid> _guidePaymentService;
        private readonly IPaymentBookingService<BusBooking, Booking, Guid> _busPaymentService;
        private readonly IPaymentBookingService<CarBooking, Booking, Guid> _carPaymentService;
        private readonly IPaymentBookingService<HotelBooking, Booking, Guid> _hotelPaymentService;
        private readonly ILogger _logger;
        private readonly IPaymentOnlyService _paymentOnlyService;
        public PaymentController(IGetService<Booking, Guid> bookingService, UserManager<User> userManager, ISessionService sessionService, IPhotographerService photographerService, IGuideService guideService, IBusService busService, ICarService carService, IHotelService hotelService, ILogger logger, IPaymentBookingService<PhotographerBooking, Booking, Guid> photographerPaymentService, IPaymentBookingService<GuideBooking, Booking, Guid> guidePaymentService, IPaymentBookingService<BusBooking, Booking, Guid> busPaymentService, IPaymentBookingService<CarBooking, Booking, Guid> carPaymentService, IPaymentBookingService<HotelBooking, Booking, Guid> hotelPaymentService, IPaymentOnlyService paymentOnlyService)
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
            _photographerPaymentService = photographerPaymentService;
            _guidePaymentService = guidePaymentService;
            _busPaymentService = busPaymentService;
            _carPaymentService = carPaymentService;
            _hotelPaymentService = hotelPaymentService;
            _paymentOnlyService = paymentOnlyService;
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
            PostData.Add("tran_id", Guid.NewGuid().ToString());
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
            var storeId = "dynam680284d88fcf0";
            var storePassword = "dynam680284d88fcf0@ssl";
            var isSandboxMood = true;

            SSLCommerzGatewayProcessor sslcz = new SSLCommerzGatewayProcessor(storeId, storePassword, isSandboxMood);

            string response = sslcz.InitiateTransaction(PostData);
            if(string.IsNullOrEmpty(_sessionService.GetString("EventDate")))
            {
                _logger.LogError($"PaymentController:Pay: EventDate is empty. Redirecting to Expired action");
                return RedirectToAction("Redirect");
            }
            if (response.Contains("Confirmation"))
            {
                return RedirectToAction(response, "Payment", new { id });
            }
            return RedirectToAction(response);

        }

        public async Task<IActionResult> Confirmation(Guid id)
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

            var user = await _userManager.GetUserAsync(User);
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
                    bookingId = BookPhotographer(photogrpaher,user);
                }
                else if (guide != null)
                {
                    bookingId = BookGuide(guide, user);
                }
                else if (bus != null)
                {
                    bookingId = BookBus(bus, user);
                }
                else if (car != null)
                {
                    bookingId = BookCar(car, user);
                }
                else if (hotel != null)
                {
                    bookingId = BookHotel(hotel, user);
                }
            }
            AddPayment(bookingId, Guid.Parse(TrxID));
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
        public void AddPayment(Guid bookingId, Guid transactionId)
        {
            _paymentOnlyService.AddPaymentOnly(new Payment
            {
                BookingId = bookingId,
                TransactionId = transactionId,
                PaymentStatus = PaymentStatus.Completed,
                PaymentMethod = PaymentMethods.SSLCommerz,
            });
        }
        public Guid BookPhotographer(Photographer photographer,User user)
        {
            var pgBooking = new PhotographerBooking
            {
                UserName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture),
                PhotographerId = photographer.Id,
            };
            var booking = new Booking
            {
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = photographer.HourlyRate * pgBooking.TimeInHour,
                UserId = user.Id,
            };
            return _photographerPaymentService.AddPayment(pgBooking,booking);
        }
        public Guid BookGuide(Guide guide, User user)
        {
            var gdBooking = new GuideBooking
            {
                UserName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture),
                GuideId = guide.Id,
            };
            var booking = new Booking
            {
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = guide.HourlyRate * gdBooking.TimeInHour,
                UserId = user.Id,
            };
            return _guidePaymentService.AddPayment(gdBooking, booking);
        }
        public Guid BookBus(Bus bus, User user)
        {
            var booking = new Booking
            {
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                //TotalAmount = photographer.HourlyRate * pgBooking.TimeInHour,
                UserId = user.Id,
            };
            //return _photographerPaymentService.AddPayment(pgBooking, booking);
            return Guid.Empty;
        }
        public Guid BookCar(Car car, User user)
        {
            var pgBooking = new PhotographerBooking
            {
                UserName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture),
                PhotographerId = car.Id,
            };
            var booking = new Booking
            {
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                //TotalAmount = photographer.HourlyRate * pgBooking.TimeInHour,
                UserId = user.Id,
            };
            return _photographerPaymentService.AddPayment(pgBooking, booking);
        }
        public Guid BookHotel(Hotel hotel, User user)
        {
            //var booking = _bookingService.Get(id);
            //if (booking != null && booking.BookingTypes == BookingTypes.Hotel)
            //{
            //}
            //return null;
            return Guid.Empty;
        }
    }
}
