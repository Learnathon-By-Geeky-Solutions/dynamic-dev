using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Controllers;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EasyTravel.Test
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IBookingService> _mockBookingService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<ISessionService> _mockSessionService;
        private Mock<IBusService> _mockBusService;
        private Mock<ICarService> _mockCarService;
        private Mock<IPhotographerBookingService> _mockPhotographerBookingService;
        private Mock<IGuideBookingService> _mockGuideBookingService;
        private Mock<ILogger<PaymentController>> _mockLogger;
        private Mock<IPaymentOnlyService> _mockPaymentOnlyService;
        private Mock<IConfiguration> _mockConfig;
        private Mock<IHotelBookingService> _mockHotelBookingService;
        private PaymentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockBookingService = new Mock<IBookingService>();
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!
            );
            _mockSessionService = new Mock<ISessionService>();
            _mockBusService = new Mock<IBusService>();
            _mockCarService = new Mock<ICarService>();
            _mockPhotographerBookingService = new Mock<IPhotographerBookingService>();
            _mockGuideBookingService = new Mock<IGuideBookingService>();
            _mockLogger = new Mock<ILogger<PaymentController>>();
            _mockPaymentOnlyService = new Mock<IPaymentOnlyService>();
            _mockConfig = new Mock<IConfiguration>();
            _mockHotelBookingService = new Mock<IHotelBookingService>();
            _controller = new PaymentController(
                _mockBookingService.Object,
                _mockUserManager.Object,
                _mockSessionService.Object,
                _mockBusService.Object,
                _mockCarService.Object,
                _mockLogger.Object,
                _mockPhotographerBookingService.Object,
                _mockGuideBookingService.Object,
                _mockPaymentOnlyService.Object,
                _mockConfig.Object,
                _mockHotelBookingService.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            if (_controller is IDisposable disposableController)
            {
                disposableController.Dispose();
            }
        }

        [Test]
        public async Task Pay_ReturnsRedirectToExpired_WhenBookingAlreadyExists()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var booking = new Booking { Id = bookingId, TotalAmount = 100, BookingTypes = BookingTypes.Bus };
            _mockBookingService.Setup(s => s.Get(bookingId)).Returns(booking);
            _mockPaymentOnlyService.Setup(s => s.IsExist(bookingId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Pay(new BookingModel { Id = bookingId }, bookingId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectResult>());
            var redirectResult = result as RedirectResult;
            Assert.That(redirectResult?.Url, Is.EqualTo("/Pay/Expired"));
        }

        [Test]
        public async Task Pay_ReturnsRedirectToGateway_WhenPaymentInitializedSuccessfully()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var booking = new BookingModel();
            var hotelBooking = new Booking
            {
                Id = bookingId,
                TotalAmount = 100,
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Hotel,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = Guid.NewGuid(),
            };
            _mockBookingService.Setup(s => s.Get(bookingId)).Returns(hotelBooking);
            _mockPaymentOnlyService.Setup(s => s.IsExist(bookingId)).ReturnsAsync(false);
            _mockSessionService.Setup(s => s.GetString("TotalAmount")).Returns("100");
            _mockSessionService.Setup(s => s.GetString("BookingType")).Returns("Bus");

            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(c => c["StoreId"]).Returns("test_store_id");
            mockConfigSection.Setup(c => c["StorePassword"]).Returns("test_store_password");
            mockConfigSection.Setup(c => c["IsSandbox"]).Returns("true");

            _mockConfig.Setup(c => c.GetSection("SSLCommerz")).Returns(mockConfigSection.Object);

            // Act
            var result = await _controller.Pay(booking, bookingId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectResult>());
        }
    }
}
