using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Test.InfrastructureTests.RepositoryTests;
[TestFixture]
public class SeatRepositoryTests
{
    private ApplicationDbContext _context;
    private SeatRepository _repository;


    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Seat")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new SeatRepository(_context);

        // Seed data
        var bus = new Bus
        {
            Id = Guid.NewGuid(),
            OperatorName = "Operator 1",
            BusType = "Luxury", // Add this line to set the required property
            From = "City A",
            To = "City B",
            DepartureTime = DateTime.Now.AddHours(1),
            ArrivalTime = DateTime.Now.AddHours(5),
            Price = 50.00m,
            TotalSeats = 28
        };

        _context.Buses.Add(bus);

        _context.Seats.AddRange(new List<Seat>
    {
        new Seat
        {
            Id = Guid.NewGuid(),
            BusId = bus.Id,
            SeatNumber = "A1",
            IsAvailable = true
        },
        new Seat
        {
            Id = Guid.NewGuid(),
            BusId = bus.Id,
            SeatNumber = "A2",
            IsAvailable = false
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
public void GetAllSeats_ShouldReturnAllSeats()
{
    // Act
    var result = _repository.GetAll().ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result.Count, Is.EqualTo(2), "The count of seats is incorrect."); // Use Count property
        Assert.That(result[0].SeatNumber, Is.EqualTo("A1"), "The first seat's number is incorrect.");
    });
}
}

