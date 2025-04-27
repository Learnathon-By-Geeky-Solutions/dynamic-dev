using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EasyTravel.Test.ApplicationsTests.ServiceTests
{
    [TestFixture]
    public class AdminCarBookingServiceTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<AdminCarBookingService>> _loggerMock;
        private AdminCarBookingService _adminCarBookingService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _loggerMock = new Mock<ILogger<AdminCarBookingService>>();
            _adminCarBookingService = new AdminCarBookingService(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Get_ShouldReturnCarBooking_WhenCarBookingExists()
        {
            // Arrange
            var carBookingId = Guid.NewGuid();
            var carBooking = new CarBooking
            {
                Id = carBookingId,
                CarId = Guid.NewGuid(),
                PassengerName = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "123-456-7890",
                BookingDate = DateTime.Now
            };
            _unitOfWorkMock.Setup(u => u.CarBookingRepository.GetById(carBookingId)).Returns(carBooking);

            // Act
            var result = _adminCarBookingService.Get(carBookingId);

            // Assert
            Assert.That(result, Is.EqualTo(carBooking), "The returned car booking should match the expected car booking.");
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains($"Fetching car booking with ID: {carBookingId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Test]
        public void Get_ShouldThrowKeyNotFoundException_WhenCarBookingDoesNotExist()
        {
            // Arrange
            var carBookingId = Guid.NewGuid();
            _unitOfWorkMock.Setup(u => u.CarBookingRepository.GetById(carBookingId)).Returns((CarBooking)null);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => _adminCarBookingService.Get(carBookingId));

            // Assert
            Assert.Multiple(() =>
            {
         
            Assert.That(ex, Is.Not.Null, "An exception should be thrown.");
            Assert.That(ex.Message, Is.EqualTo($"An error occurred while fetching the car booking with ID: {carBookingId}."));
            Assert.That(ex.InnerException, Is.TypeOf<KeyNotFoundException>(), "The inner exception should be a KeyNotFoundException.");
            Assert.That(ex.InnerException?.Message, Is.EqualTo($"Car booking with ID: {carBookingId} not found."));
            });
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains($"Car booking with ID: {carBookingId} not found.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains($"An error occurred while fetching the car booking with ID: {carBookingId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }


        [Test]
        public void GetAll_ShouldReturnAllCarBookings()
        {
            // Arrange
            var carBookings = new List<CarBooking>
            {
                new CarBooking
                {
                    Id = Guid.NewGuid(),
                    CarId = Guid.NewGuid(),
                    PassengerName = "John Doe",
                    Email = "johndoe@example.com",
                    PhoneNumber = "123-456-7890",
                    BookingDate = DateTime.Now
                },
                new CarBooking
                {
                    Id = Guid.NewGuid(),
                    CarId = Guid.NewGuid(),
                    PassengerName = "Jane Smith",
                    Email = "janesmith@example.com",
                    PhoneNumber = "987-654-3210",
                    BookingDate = DateTime.Now
                }
            };
            _unitOfWorkMock.Setup(u => u.CarBookingRepository.GetAll()).Returns(carBookings);

            // Act
            var result = _adminCarBookingService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(carBookings), "The returned car bookings should match the expected list.");
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Fetching all car bookings.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Test]
        public void Delete_ShouldRemoveCarBookingAndSave()
        {
            // Arrange
            var carBookingId = Guid.NewGuid();
            var bookingRepositoryMock = new Mock<IBookingRepository>();
            _unitOfWorkMock.Setup(u => u.BookingRepository).Returns(bookingRepositoryMock.Object);

            // Act
            _adminCarBookingService.Delete(carBookingId);

            // Assert
            bookingRepositoryMock.Verify(r => r.Remove(carBookingId), Times.Once, "Car booking should be removed from the repository.");
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once, "Save should be called after removing the car booking.");
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains($"Attempting to delete car booking with ID: {carBookingId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains($"Successfully deleted car booking with ID: {carBookingId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Test]
        public void Delete_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _adminCarBookingService.Delete(Guid.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Invalid car booking ID provided for deletion.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}


