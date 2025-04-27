using EasyTravel.Infrastructure.Data;
using EasyTravel.Domain.Entites;
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
    public class BusRepositoryTests
    {
        private ApplicationDbContext _context;
        private BusRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Bus")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new BusRepository(_context);

            // Seed data
            _context.Buses.AddRange(new List<Bus>
            {
                new Bus
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
                },
                new Bus
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
                }
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetAllBuses_ShouldReturnAllBuses()
        {
            // Act
            var result = _repository.GetAllBuses().ToList(); // Ensure result is a list

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2), "The count of buses is incorrect."); // Updated
                Assert.That(result[0].OperatorName, Is.EqualTo("Operator 1"), "The first operator's name is incorrect.");
            });
        }

        [Test]
        public void AddBus_ShouldAddBus()
        {
            // Arrange
            var bus = new Bus
            {
                Id = Guid.NewGuid(),
                OperatorName = "Operator 3",
                BusType = "Mini",
                From = "City E",
                To = "City F",
                DepartureTime = DateTime.Now.AddHours(3),
                ArrivalTime = DateTime.Now.AddHours(7),
                Price = 40.00m,
                TotalSeats = 20
            };

            // Act
            _repository.Addbus(bus);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetAllBuses().ToList(); // Ensure result is a list
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3), "The count of buses is incorrect after addition."); // Updated
                Assert.That(result.Any(b => b.OperatorName == "Operator 3"), Is.True, "The added bus is missing.");
            });
        }
    }
}