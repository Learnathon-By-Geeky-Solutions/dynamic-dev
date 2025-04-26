using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Test.Repositories
{
    [TestFixture]
    public class BusBookingRepositoryTests
    {
        private ApplicationDbContext _context;
        private BusBookingRepository _repository;


        [SetUp]
        public void SetUp()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new BusBookingRepository(_context);

            // Seed buses
            var bus1 = new Bus
            {
                Id = Guid.NewGuid(),
                OperatorName = "Operator 1",
                BusType = "Luxury",
                From = "City A",
                To = "City B",
                DepartureTime = DateTime.Now.AddHours(1),
                ArrivalTime = DateTime.Now.AddHours(5),
                Price = 50.00m,
                TotalSeats = 28
            };

            var bus2 = new Bus
            {
                Id = Guid.NewGuid(),
                OperatorName = "Operator 2",
                BusType = "Standard",
                From = "City C",
                To = "City D",
                DepartureTime = DateTime.Now.AddHours(2),
                ArrivalTime = DateTime.Now.AddHours(6),
                Price = 30.00m,
                TotalSeats = 28
            };

            _context.Buses.AddRange(bus1, bus2);

            // Seed bus bookings
            _context.BusBookings.AddRange(new List<BusBooking>
    {
        new BusBooking
        {
            Id = Guid.NewGuid(),
            PassengerName = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            BusId = bus1.Id,
            SelectedSeats = new List<string> { "A1", "A2" },
            SelectedSeatIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        },
        new BusBooking
        {
            Id = Guid.NewGuid(),
            PassengerName = "Jane Doe",
            Email = "jane.doe@example.com",
            PhoneNumber = "987-654-3210",
            BusId = bus2.Id,
            SelectedSeats = new List<string> { "B1", "B2" },
            SelectedSeatIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        }
    });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetAllBusBookings_ShouldReturnAllBusBookings()
        {
            // Act
            var result = _repository.GetAllBusBookings();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2)); // 2 seeded bus bookings
            Assert.That(result.First().PassengerName, Is.EqualTo("John Doe"));
            Assert.That(result.First().Email, Is.EqualTo("john.doe@example.com"));
        }

        [Test]
        public void DeleteBusBooking_ShouldRemoveBusBooking_WhenBusBookingExists()
        {
            // Arrange
            var busBookingToDelete = _context.BusBookings.First();

            // Act
            _repository.DeleteBusBooking(busBookingToDelete.Id);

            // Assert
            var result = _repository.GetAllBusBookings();
            Assert.That(result.Count(), Is.EqualTo(1)); // 1 remaining after deletion
            Assert.That(result.Any(b => b.Id == busBookingToDelete.Id), Is.False);
        }
    }
}
