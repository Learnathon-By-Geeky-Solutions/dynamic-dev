using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Application.Services;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using EasyTravel.Domain.Services;


namespace EasyTravel.Test
{
    [TestFixture]
    public class BusServiceTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<IBusRepository> _mockBusRepository;
        private Mock<ILogger<BusService>> _mockLogger;
        private Mock<IBookingService> _mockBookingService; // Add mock for IBookingService
        private BusService _busService;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _mockBusRepository = new Mock<IBusRepository>();
            _mockLogger = new Mock<ILogger<BusService>>();
            _mockBookingService = new Mock<IBookingService>(); // Initialize the mock

            _mockUnitOfWork.Setup(u => u.BusRepository).Returns(_mockBusRepository.Object);
            _busService = new BusService(_mockUnitOfWork.Object, _mockLogger.Object, _mockBookingService.Object); // Pass the mockBookingService
        }

        // Other test methods remain unchanged
    }
}