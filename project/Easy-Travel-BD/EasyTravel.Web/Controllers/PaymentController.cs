using EasyTravel.Domain.Entites;

using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using User = EasyTravel.Domain.Entites.User;

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
        private readonly IPhotographerBookingService _photographerBookingService;
        private readonly IGuideBookingService _guideBookingService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentOnlyService _paymentOnlyService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public PaymentController(IGetService<Booking, Guid> bookingService, UserManager<User> userManager, ISessionService sessionService, IPhotographerService photographerService, IGuideService guideService, IBusService busService, ICarService carService, IHotelService hotelService, ILogger<PaymentController> logger,IPhotographerBookingService photographerBookingService,IGuideBookingService guideBookingService, IPaymentOnlyService paymentOnlyService, IConfiguration config, IWebHostEnvironment env)
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
            _photographerBookingService = photographerBookingService;
            _guideBookingService = guideBookingService;
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
                _sessionService.SetString("BookingId", bookingmodel.Id.ToString());
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
            if(photographer != null)
            {
                var pgBookingModel = JsonSerializer.Deserialize<PhotographerBookingViewModel>(_sessionService.GetString("photographerBookingObj"));
                totalAmount = pgBookingModel?.TotalAmount.ToString();
                bookingType = BookingTypes.Photographer.ToString();
                _sessionService.SetString("BookingId", photographer.Id.ToString());
            }
            else if (guide != null)
            {
                var guideBookingModel = JsonSerializer.Deserialize<GuideBookingViewModel>(_sessionService.GetString("guideBookingObj"));
                totalAmount = guideBookingModel?.TotalAmount.ToString();
                bookingType = BookingTypes.Guide.ToString();
                _sessionService.SetString("BookingId", guide.Id.ToString());
            }
            else if (bus != null)
            {
                var busBookingModel = JsonSerializer.Deserialize<BusBookingViewModel>(_sessionService.GetString("busBookingObj"));
                totalAmount = busBookingModel?.TotalAmount.ToString();
                bookingType = BookingTypes.Bus.ToString();
                _sessionService.SetString("BookingId", bus.Id.ToString());
            }
            else if (car != null)
            {
                var carBookingModel = JsonSerializer.Deserialize<CarBookingViewModel>(_sessionService.GetString("carBookingObj"));
                totalAmount = carBookingModel?.BookingForm?.TotalAmount.ToString();
                bookingType = BookingTypes.Car.ToString();
                _sessionService.SetString("BookingId", car.Id.ToString());
            }
            else if (hotel != null)
            {
                var hotelBookingModel = JsonSerializer.Deserialize<HotelBookingViewModel>(_sessionService.GetString("hotelBookingObj"));
                //totalAmount = hotelBookingModel..ToString(); // Need to pass total amount from hotel booking view model
                bookingType = BookingTypes.Hotel.ToString();
                _sessionService.SetString("BookingId", hotel.Id.ToString());
            }
            var user = await _userManager.GetUserAsync(User);

            var configSection = _config.GetSection("SSLCommerz");
            var baseUrl = Request.Scheme + "://" + Request.Host;
            var request = new SSLCommerzRequest
            {
                StoreId = configSection["StoreId"]!,
                StorePassword = configSection["StorePassword"]!,
                TotalAmount = totalAmount!.ToString(),
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
        public IActionResult Success()
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SuccessInfo = "Invalid request data";
                return View();
            }
            var id = Guid.Parse(_sessionService.GetString("BookingId"));
            var user = _userManager.GetUserAsync(User).Result;
            var transactionId = Request.Form["tran_id"];
            var bookingmodel = _bookingService.Get(id);
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                TransactionId = Guid.Parse(transactionId!),
                PaymentStatus = PaymentStatus.Completed,
                PaymentMethod = PaymentMethods.SSLCommerz,
            };
            if (bookingmodel != null)
            {
                var booking = _paymentOnlyService.IsExist(bookingmodel.Id).Result;
                if (booking)
                {
                    _logger.LogError($"PaymentController:Success: Booking already exists. Redirecting to Expired action");
                    return RedirectToAction("Expired", "Payment");
                }
                AddPayment(bookingmodel, payment);
                return View();
            }
            var photographer = _photographerService.Get(id);
            var guide = _guideService.Get(id);
            var hotel = _hotelService.Get(id);
            var bus = _busService.GetBusById(id);
            var car = _carService.GetCarById(id);
            if (photographer != null && user != null)
            {
                BookPhotographer(user, payment);
            }
            else if (guide != null && user != null)
            {
                BookGuide(user, payment);
            }
            else if (hotel != null && user != null)
            {
                BookHotel(user, payment);
            }
            else if (bus != null && user != null)
            {
                BookBus(user, payment);
            }
            else if (car != null && user != null)
            {
                BookCar(user, payment);
            }

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
        public void AddPayment(Booking booking, Payment payment)
        {
            booking.BookingStatus = BookingStatus.Confirmed;
            payment.Amount = booking.TotalAmount;
            payment.BookingId = booking.Id;
            _paymentOnlyService.AddPaymentOnly(payment);
        }
        public void BookPhotographer(User user,Payment payment)
        {
            var pgBookingModel = JsonSerializer.Deserialize<PhotographerBookingViewModel>(_sessionService.GetString("photographerBookingObj"));
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = pgBookingModel.TotalAmount,
                UserId = user.Id,
            };
            var pgBooking = new PhotographerBooking
            {
                Id = booking.Id,
                UserName = $"{pgBookingModel.FirstName} {pgBookingModel.LastName}",
                Email = pgBookingModel.Email,
                PhoneNumber = pgBookingModel.PhoneNumber,
                Gender = pgBookingModel.Gender,
                EventDate = pgBookingModel.EventDate,
                StartTime = pgBookingModel.StartTime!,
                EndTime = pgBookingModel.EndTime,
                TimeInHour = pgBookingModel.TimeInHour,
                PhotographerId = pgBookingModel.PhotographerId,
            };
            payment.Amount = pgBookingModel.TotalAmount;
            payment.BookingId = booking.Id;
            _photographerBookingService.SaveBooking(pgBooking, booking,payment);
        }
        public void BookGuide(User user, Payment payment)
        {
            var guideBookingModel = JsonSerializer.Deserialize<GuideBookingViewModel>(_sessionService.GetString("guideBookingObj"));
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = guideBookingModel!.TotalAmount,
                UserId = user.Id,
            };
            var gdBooking = new GuideBooking
            {
                Id = booking.Id,
                UserName = $"{guideBookingModel.FirstName} {guideBookingModel.LastName}",
                Email = guideBookingModel.Email,
                PhoneNumber = guideBookingModel.PhoneNumber,
                Gender = guideBookingModel.Gender,
                EventDate = guideBookingModel.EventDate,
                StartTime = guideBookingModel.StartTime!,
                EndTime = guideBookingModel.EndTime,
                TimeInHour = guideBookingModel.TimeInHour,
                GuideId = guideBookingModel.GuideId,
            };
            payment.Amount = guideBookingModel.TotalAmount;
            payment.BookingId = booking.Id;
            _guideBookingService.SaveBooking(gdBooking, booking,payment);
        }
        public void BookBus(User user, Payment payment)
        {
            var busBookingModel = JsonSerializer.Deserialize<BusBookingViewModel>(_sessionService.GetString("busBookingObj"));
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = busBookingModel!.TotalAmount,
                UserId = user.Id,
            };
            var busBooking = new BusBooking
            {
                Id = booking.Id,
                BusId = busBookingModel.BusId,
                PassengerName = busBookingModel.BookingForm.PassengerName,
                Email = busBookingModel.BookingForm.Email,
                PhoneNumber = busBookingModel.BookingForm.PhoneNumber,
                BookingDate = DateTime.Now,
                SelectedSeatIds = busBookingModel.SelectedSeatIds,
                SelectedSeats = busBookingModel.SelectedSeatNumbers,
            };
            payment.Amount = busBookingModel.TotalAmount;
            payment.BookingId = booking.Id;
            _busService.SaveBooking(busBooking,busBooking.SelectedSeatIds!, booking, payment);
        }
        public void BookCar(User user,Payment payment)
        {
            var carBookingModel = JsonSerializer.Deserialize<CarBookingViewModel>(_sessionService.GetString("carBookingObj"));
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Photographer,
                TotalAmount = carBookingModel!.BookingForm!.TotalAmount,
                UserId = user.Id,
            };
            var carBooking = new CarBooking
            {
                Id = booking.Id,
                CarId = carBookingModel.CarId,
                PassengerName = carBookingModel.BookingForm.PassengerName,
                Email = carBookingModel.BookingForm.Email,
                PhoneNumber = carBookingModel.BookingForm.PhoneNumber,
                BookingDate = DateTime.Now,
            };
            payment.Amount = booking.TotalAmount;
            payment.BookingId = booking.Id;
            _carService.SaveBooking(carBooking,carBooking.CarId, booking, payment);
        }
        public void BookHotel(User user, Payment payment)
        {
        }
    }
}
