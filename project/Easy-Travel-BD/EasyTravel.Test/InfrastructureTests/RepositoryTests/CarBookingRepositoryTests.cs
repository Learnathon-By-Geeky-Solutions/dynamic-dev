using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class CarBookingRepositoryTests
{
    private ApplicationDbContext _context;
    private CarBookingRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_CarBooking")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CarBookingRepository(_context);

        // Seed data
        var car = new Car
        {
            Id = Guid.NewGuid(),
            OperatorName = "Operator 1",
            CarType = "SUV",
            From = "City A",
            To = "City B",
            DepartureTime = DateTime.Now.AddHours(1),
            ArrivalTime = DateTime.Now.AddHours(5),
            Price = 100.00m,
            IsAvailable = true
        };

        _context.Cars.Add(car);

        _context.CarBookings.AddRange(new List<CarBooking>
        {
            new CarBooking
            {
                Id = Guid.NewGuid(),
                PassengerName = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                CarId = car.Id,
                BookingDate = DateTime.Now
            },
            new CarBooking
            {
                Id = Guid.NewGuid(),
                PassengerName = "Jane Doe",
                Email = "jane.doe@example.com",
                PhoneNumber = "987-654-3210",
                CarId = car.Id,
                BookingDate = DateTime.Now
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
    public void GetAllCarBookings_ShouldReturnAllCarBookings()
    {
        var result = _repository.GetAll();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().PassengerName, Is.EqualTo("John Doe"));
    }
}
