using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

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
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentOnlyService _paymentOnlyService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public PaymentController(IGetService<Booking, Guid> bookingService, UserManager<User> userManager, ISessionService sessionService, IPhotographerService photographerService, IGuideService guideService, IBusService busService, ICarService carService, IHotelService hotelService, ILogger<PaymentController> logger, IPaymentBookingService<PhotographerBooking, Booking, Guid> photographerPaymentService, IPaymentBookingService<GuideBooking, Booking, Guid> guidePaymentService, IPaymentBookingService<BusBooking, Booking, Guid> busPaymentService, IPaymentBookingService<CarBooking, Booking, Guid> carPaymentService, IPaymentBookingService<HotelBooking, Booking, Guid> hotelPaymentService, IPaymentOnlyService paymentOnlyService, IConfiguration config, IWebHostEnvironment env)
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
            _config = config;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Pay(Guid id)
        {
            var totalAmount = _sessionService.GetString("TotalAmount");
            var bookingType = _sessionService.GetString("BookingType");
            var bookingmodel = _bookingService.Get(id);

            if (bookingmodel != null)
            {
                var booking = await _paymentOnlyService.IsExist(bookingmodel.Id);
                if (booking)
                {
                    _logger.LogError($"PaymentController:Pay: Booking already exists. Redirecting to Expired action");
                    return RedirectToAction("Expired", "Payment");
                }
                totalAmount = bookingmodel.TotalAmount.ToString();
                bookingType = bookingmodel.BookingTypes.ToString();
            }
            var photographer = _photographerService.Get(id);
            var guide = _guideService.Get(id);
            var hotel = _hotelService.Get(id);
            var bus = _busService.GetBusById(id);
            var car = _carService.GetCarById(id);
            if (photographer == null && guide == null && hotel == null && bus == null && car == null)
            {
                _logger.LogError($"PaymentController:Pay: BookingType is null. Redirecting to Expired action");
                return RedirectToAction("Expired", "Payment");
            }
            var user = await _userManager.GetUserAsync(User);

            var configSection = _config.GetSection("SSLCommerz");
            var baseUrl = Request.Scheme + "://" + Request.Host;
            var request = new SSLCommerzRequest
            {
                StoreId = configSection["StoreId"],
                StorePassword = configSection["StorePassword"],
                TotalAmount = totalAmount.ToString(),
                TranId = Guid.NewGuid().ToString("N"),
                SuccessUrl = baseUrl + "/Payment/Success",
                FailUrl = baseUrl + "/Payment/Fail",
                CancelUrl = baseUrl + "/Payment/Cancel",
                CusName = $"{user?.FirstName},{user?.LastName}",
                CusEmail = $"{user.Email}",
                CusPhone = $"{user.PhoneNumber}",
                CusAdd1 = "Dhaka",
                CusCity = "Dhaka",
            };

            string gatewayUrl = configSection.GetValue<bool>("IsSandbox")
                ? "https://sandbox.sslcommerz.com/gwprocess/v4/api.php"
                : "https://securepay.sslcommerz.com/gwprocess/v4/api.php";
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "store_id", request.StoreId },
        { "store_passwd", request.StorePassword },
        { "total_amount", request.TotalAmount },
        { "currency", request.Currency },
        { "tran_id", request.TranId },
        { "success_url", request.SuccessUrl },
        { "fail_url", request.FailUrl },
        { "cancel_url", request.CancelUrl },
        { "cus_name", request.CusName },
        { "cus_email", request.CusEmail },
        { "cus_add1", request.CusAdd1 },
        { "cus_city", request.CusCity },
        { "cus_country", request.CusCountry },
        { "cus_phone", request.CusPhone },

        // ✅ Required for Payment Initialization
        { "shipping_method", "NO" },         // No physical shipping
        { "num_of_item", "1" },              // Just send 1 if unsure
        { "product_name", "Test Product" },
        { "product_category", $"{bookingType}" },
        { "product_profile", "general" },
    });

            var response = await client.PostAsync(gatewayUrl, content);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (result != null && result.TryGetValue("GatewayPageURL", out var url) && !string.IsNullOrEmpty(url?.ToString()))
                {
                    return Redirect(url.ToString());
                }

                return Content($"Payment initialization failed. Response: {json}");
            }
            catch (Exception ex)
            {
                return Content($"Exception occurred: {ex.Message}\nRaw Response: {json}");
            }
        }
        [AllowAnonymous]
        public IActionResult Success(Guid id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SuccessInfo = "Invalid request data";
                return View();
            }
            var bookingId = id;
            var transactionId = Request.Form["tran_id"];
            var photographer = _photographerService.Get(bookingId);
            var guide = _guideService.Get(bookingId);
            var hotel = _hotelService.Get(bookingId);
            var bus = _busService.GetBusById(bookingId);
            var car = _carService.GetCarById(bookingId);
            var user = _userManager.GetUserAsync(User).Result;
            if (photographer != null && user != null)
            {
                bookingId = BookPhotographer(photographer, user);
            }
            else if (guide != null && user != null)
            {
                bookingId = BookGuide(guide, user);
            }
            else if (hotel != null && user != null)
            {
                bookingId = BookHotel(hotel, user);
            }
            else if (bus != null && user != null)
            {
                bookingId = BookBus(bus, user);
            }
            else if (car != null && user != null)
            {
                bookingId = BookCar(car, user);
            }
            AddPayment(bookingId, Guid.Parse(transactionId!));
            ViewBag.SuccessInfo = "Your payment was successful. Thank you for your booking!";
            return View();
        }
        [AllowAnonymous]
        public IActionResult Fail(Guid id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SuccessInfo = "Invalid request data";
                return View();
            }
            ViewBag.FailInfo = "Your payment was not successful. Please try again.";
            return View();
        }
        [AllowAnonymous]
        public IActionResult Cancel(Guid id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SuccessInfo = "Invalid request data";
                return View();
            }
            ViewBag.CancelInfo = "Your payment was cancelled. Please try again.";
            return View();
        }
        public IActionResult Expired()
        {
            ViewBag.ExpireInfo = "Your payment session has expired";
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
        public Guid BookPhotographer(Photographer photographer, User user)
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
            return _photographerPaymentService.AddPayment(pgBooking, booking);
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
                BookingTypes = BookingTypes.Bus,
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
            var hotelbooking = new HotelBooking
            {
                HotelId = hotel.Id,
            };
            return Guid.Empty;
        }
    }
}
