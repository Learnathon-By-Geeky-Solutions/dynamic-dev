using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyTravel.Tests.Services
{
    [TestFixture]
    public class BusServiceTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<BusService>> _loggerMock = null!;
        private Mock<IBookingService> _bookingServiceMock = null;
        private BusService _busService = null!;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _loggerMock = new Mock<ILogger<BusService>>();
            _bookingServiceMock = new Mock<IBookingService>();
            _busService = new BusService(_unitOfWorkMock.Object, _loggerMock.Object,_bookingServiceMock.Object);
        }

        [Test]
        public void CreateBus_ShouldThrowArgumentNullException_WhenBusIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _busService.CreateBus(null!));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex!.ParamName, Is.EqualTo("bus"));
                Assert.That(ex.Message, Does.Contain("Bus entity cannot be null."));
            });
        }

        [Test]
        public void CreateBus_ShouldAddBusAndSave_WhenBusIsValid()
        {
            // Arrange
            var bus = new Bus
            {
                Id = Guid.NewGuid(),
                OperatorName = "Test Operator",
                From = "CityA",
                To = "CityB",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(2),
                Price = 100,
                TotalSeats = 28,
                Seats = new List<Seat>()
            };

            _unitOfWorkMock.Setup(u => u.BusRepository.Addbus(bus));
            _unitOfWorkMock.Setup(u => u.Save());

            // Act
            _busService.CreateBus(bus);

            // Assert
            Assert.Multiple(() =>
            {
                _unitOfWorkMock.Verify(u => u.BusRepository.Addbus(bus), Times.Once);
                _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
                Assert.That(bus.Seats.Count, Is.EqualTo(28), "Seats should be initialized correctly.");
            });
        }

        [Test]
        public void GetBusById_ShouldThrowArgumentException_WhenBusIdIsEmpty()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _busService.GetBusById(Guid.Empty));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex!.ParamName, Is.EqualTo("BusId"));
                Assert.That(ex.Message, Does.Contain("Bus ID cannot be empty."));
            });
        }

        [Test]
        public void GetBusById_ShouldReturnBus_WhenBusExists()
        {
            // Arrange
            var busId = Guid.NewGuid();
            var bus = new Bus
            {
                Id = busId,
                OperatorName = "Test Operator",
                From = "CityA",
                To = "CityB",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(2),
                Price = 100,
                TotalSeats = 28
            };

            _unitOfWorkMock.Setup(u => u.BusRepository.GetById(busId)).Returns(bus);

            // Act
            var result = _busService.GetBusById(busId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(busId));
                Assert.That(result.OperatorName, Is.EqualTo("Test Operator"));
            });
        }

        [Test]
        public async Task GetAvailableBusesAsync_ShouldReturnAvailableBuses()
        {
            // Arrange
            var from = "CityA";
            var to = "CityB";
            var dateTime = DateTime.Now.Date;
            var pageNumber = 1;
            var pageSize = 1;

            var buses = new List<Bus>
            {
                new Bus
                {
                    Id = Guid.NewGuid(),
                    OperatorName = "Operator1",
                    From = from,
                    To = to,
                    DepartureTime = dateTime,
                    ArrivalTime = dateTime.AddHours(2),
                    Price = 100,
                    TotalSeats = 28,
                    Seats = new List<Seat>
                    {
                        new Seat
                        {
                            Id = Guid.NewGuid(),
                            BusId = Guid.NewGuid(),
                            SeatNumber = "A1",
                            IsAvailable = true
                        }
                    }
                }
            };

            _unitOfWorkMock.Setup(u => u.BusRepository.GetAsync(
                It.IsAny<Expression<Func<Bus, bool>>>(),
                null // Include parameter
            )).ReturnsAsync(buses);

            // Act
            var (result, totalPages) = await _busService.GetAvailableBusesAsync(from, to, dateTime, pageNumber, pageSize);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(1)); // Use Count() for IEnumerable
                Assert.That(result.First().From, Is.EqualTo(from));
                Assert.That(result.First().To, Is.EqualTo(to));
                Assert.That(totalPages, Is.EqualTo(1)); // Only one page of results
            });

        }

        [Test]
        public void GetseatBusById_ShouldReturnBusWithSeats_WhenBusExists()
        {
            // Arrange
            var busId = Guid.NewGuid();
            var bus = new Bus
            {
                Id = busId,
                OperatorName = "Test Operator",
                From = "CityA",
                To = "CityB",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(2),
                Price = 100,
                TotalSeats = 28,
                Seats = new List<Seat>
                {
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        BusId = busId,
                        SeatNumber = "A1",
                        IsAvailable = true
                    }
                }
            };

            _unitOfWorkMock.Setup(u => u.BusRepository.GetBuses())
                .Returns(new List<Bus> { bus }.AsQueryable());

            // Act
            var result = _busService.GetseatBusById(busId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(busId));
                Assert.That(result.Seats, Is.Not.Null);
                Assert.That(result.Seats, Has.Count.EqualTo(1)); // Updated to use Has.Count.EqualTo
            });
        }
    }
}
