using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Test.InfrastructureTests.RepositoryTests
{
    [TestFixture]
    public class BookingRepositoryTests
    {
        private ApplicationDbContext _context;
        private BookingRepository _repository;

        [SetUp]
        public void SetUp()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new BookingRepository(_context);

            // Seed data
            _context.Bookings.AddRange(new List<Booking>
            {
                new Booking { Id = Guid.NewGuid(), TotalAmount = 100.0m, BookingStatus = BookingStatus.Confirmed, BookingTypes = BookingTypes.Bus },
                new Booking { Id = Guid.NewGuid(), TotalAmount = 200.0m, BookingStatus = BookingStatus.Cancelled, BookingTypes = BookingTypes.Hotel }
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
        public void Add_ShouldAddBooking()
        {
            // Arrange
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                TotalAmount = 300.0m,
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Car
            };

            // Act
            _repository.Add(booking);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3), "The count of bookings is incorrect."); // Updated
                Assert.That(result.Any(b => b.TotalAmount == 300.0m), Is.True, "The added booking is missing.");
            });
        }

        [Test]
        public void GetAll_ShouldReturnAllBookings()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2), "The count of bookings is incorrect."); // Updated
                Assert.That(result[0].TotalAmount, Is.EqualTo(100.0m), "The first booking's total amount is incorrect.");
            });
        }

        [Test]
        public void Remove_ShouldRemoveBooking()
        {
            // Arrange
            var bookingToRemove = _context.Bookings.First();

            // Act
            _repository.Remove(bookingToRemove);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(1), "The count of bookings is incorrect after removal."); // Updated
                Assert.That(result.Any(b => b.Id == bookingToRemove.Id), Is.False, "The removed booking still exists.");
            });
        }
    }
}