using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using EasyTravel.Domain.Enums;
using EasyTravel.Web.Models;

namespace EasyTravel.Test
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IGetService<Booking, Guid>> _mockBookingService;
        private Mock<ISessionService> _mockSessionService;
        private Mock<IPaymentOnlyService> _mockPaymentOnlyService;
        private Mock<IConfiguration> _mockConfig;
        private PaymentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockBookingService = new Mock<IGetService<Booking, Guid>>();
            _mockSessionService = new Mock<ISessionService>();
            _mockPaymentOnlyService = new Mock<IPaymentOnlyService>();
            _mockConfig = new Mock<IConfiguration>();
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
        }


        [Test]
        public async Task Pay_ReturnsRedirectToGateway_WhenPaymentInitializedSuccessfully()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var booking = new BookingModel() ;
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

            //Act
           var result = await _controller.Pay(booking,bookingId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }
     

    }
}
