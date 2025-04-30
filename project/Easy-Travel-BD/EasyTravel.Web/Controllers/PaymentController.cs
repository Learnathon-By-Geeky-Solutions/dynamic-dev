using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using User = EasyTravel.Domain.Entites.User;

namespace EasyTravel.Web.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly UserManager<User> _userManager;
    private readonly ISessionService _sessionService;

    private readonly IBusService _busService;
    private readonly ICarService _carService;
    private readonly IHotelService _hotelService;
    private readonly IPhotographerBookingService _photographerBookingService;
    private readonly IGuideBookingService _guideBookingService;
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentOnlyService _paymentOnlyService;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    public PaymentController(IBookingService bookingService, UserManager<User> userManager, ISessionService sessionService,  IBusService busService, ICarService carService, IHotelService hotelService, ILogger<PaymentController> logger, IPhotographerBookingService photographerBookingService, IGuideBookingService guideBookingService, IPaymentOnlyService paymentOnlyService, IConfiguration config, IWebHostEnvironment env)
    {
        
        _bookingService = bookingService;
        _userManager = userManager;
        _sessionService = sessionService;
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
    public async Task<IActionResult> Pay(BookingModel bookingModel,Guid id1)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("PaymentController:Pay: Invalid model state.");
            return Redirect(_sessionService.GetString("LastVisitedPage"));
        }
        var booking = _bookingService.Get(bookingModel.Id);
        if (booking == null)
        {
            _logger.LogError($"PaymentController:Pay: Booking with ID {bookingModel.Id} not found.");
            return Redirect("/Pay/Expired");
        }
        var totalAmount = (int)Math.Floor(booking.TotalAmount);
        var user = await _userManager.GetUserAsync(User);
        var phonenumber = user?.PhoneNumber == null ? "01326759812" : user?.PhoneNumber;
        _sessionService.SetString("BookingId", booking.Id.ToString());
        var configSection = _config.GetSection("SSLCommerz");
        var baseUrl = Request.Scheme + "://" + Request.Host;
        var request = new SSLCommerzRequest
        {
            StoreId = configSection["StoreId"]!,
            StorePassword = configSection["StorePassword"]!,
            TotalAmount = totalAmount,
            TranId = Guid.NewGuid().ToString("N"),
            SuccessUrl = baseUrl + "/Payment/Success",
            FailUrl = baseUrl + "/Payment/Fail",
            CancelUrl = baseUrl + "/Payment/Cancel",
            CusName = $"{user?.FirstName},{user?.LastName}",
            CusEmail = $"{user?.Email}",
            CusPhone = $"{phonenumber}",
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
            { "total_amount", $"{request.TotalAmount}" },
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
            { "product_category", "Electronic"/*$"{bookingType}"*/ },
            { "product_profile", "general" },
            { "value_a", $"{booking.Id}" },
        });

        var response = await client.PostAsync(gatewayUrl, content);
        var json = await response.Content.ReadAsStringAsync();

        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (result != null && result.TryGetValue("GatewayPageURL", out var url) && !string.IsNullOrWhiteSpace($"{url}"))
            {
                if (bookingModel != null)
                {
                    booking.BookingStatus = BookingStatus.Confirmed;
                    if (booking.BookingTypes == BookingTypes.Photographer)
                    {
                        _photographerBookingService.SaveBooking(bookingModel.PhotographerBooking!, booking);
                    }
                    else if (booking.BookingTypes == BookingTypes.Guide)
                    {
                        _guideBookingService.SaveBooking(bookingModel.GuideBooking!, booking);
                    }
                    else if (booking.BookingTypes == BookingTypes.Bus)
                    {
                        _busService.SaveBooking(bookingModel.BusBooking!, bookingModel.BusBooking!.SelectedSeatIds!, booking);
                    }
                    else if (booking.BookingTypes == BookingTypes.Car)
                    {
                        _carService.SaveBooking(bookingModel.CarBooking!, bookingModel.CarBooking!.CarId, booking);
                    }
                    //else if (bookingmodel.BookingTypes == BookingTypes.Hotel)
                    //{
                    //    _hotelService.SaveBooking(bookingmodel.HotelBooking!, bookingmodel.CarBooking!.CarId, bookingmodel, payment);
                    //}
                }
                return Redirect($"{url}");
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

        string bookingIdStr = Request.Form["value_a"];

        Guid.TryParse(bookingIdStr, out var id);

        if (string.IsNullOrEmpty(bookingIdStr))
        {
            bookingIdStr = _sessionService.GetString("BookingId");
            _logger.LogInformation($"Using booking ID from session: {bookingIdStr}");

        }

        if (id == Guid.Empty)
        {
            _logger.LogError("No booking ID found in request or session");
            ViewBag.SuccessInfo = "No booking ID found";
            return View();
        }
        var transactionId = Request.Form["tran_id"];
        var bookingmodel = _bookingService.Get(id);
        if (bookingmodel == null)
        {
            _logger.LogError($"PaymentController:Success: Booking already exists. Redirecting to Expired action");
            return RedirectToAction("Expired", "Payment");
        }
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            TransactionId = Guid.Parse(transactionId!),
            PaymentStatus = PaymentStatus.Completed,
            PaymentMethod = PaymentMethods.SSLCommerz,
            PaymentDate = DateTime.UtcNow,
            Amount = bookingmodel.TotalAmount,
            BookingId = bookingmodel.Id,
        };
        _paymentOnlyService.AddPaymentOnly(payment);
        ViewBag.SuccessInfo = "Your payment was successful. Thank you for your booking!";
        return View();
    }

    [AllowAnonymous]
    public IActionResult Fail()
    {
        if (!ModelState.IsValid)
        {
            ViewBag.SuccessInfo = "Invalid request data";
            return View();
        }

        string bookingIdStr = Request.Form["value_a"];

        Guid.TryParse(bookingIdStr, out var id);

        if (string.IsNullOrEmpty(bookingIdStr))
        {
            bookingIdStr = _sessionService.GetString("BookingId");
            _logger.LogInformation($"Using booking ID from session: {bookingIdStr}");
        }

        if (id == Guid.Empty)
        {
            _logger.LogError("No booking ID found in request or session");
            ViewBag.SuccessInfo = "No booking ID found";
            return View();
        }
        var bookingmodel = _bookingService.Get(id);
        if (bookingmodel == null)
        {
            _logger.LogError($"PaymentController:Success: Booking already exists. Redirecting to Expired action");
            return RedirectToAction("Expired", "Payment");
        }
        if (bookingmodel.BookingStatus == BookingStatus.Confirmed)
        {
            _bookingService.RemoveBooking(id);
        }
        ViewBag.FailInfo = "Your payment was not successful. Please try again.";
        return View();
    }

    [AllowAnonymous]

    public IActionResult Cancel()
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
}
