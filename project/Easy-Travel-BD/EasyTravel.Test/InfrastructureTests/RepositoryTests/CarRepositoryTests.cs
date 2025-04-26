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
public class CarRepositoryTests
{
    private ApplicationDbContext _context;
    private CarRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Car")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CarRepository(_context);

        // Seed data
        _context.Cars.AddRange(new List<Car>
        {
            new Car
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
            },
            new Car
            {
                Id = Guid.NewGuid(),
                OperatorName = "Operator 2",
                CarType = "Sedan",
                From = "City C",
                To = "City D",
                DepartureTime = DateTime.Now.AddHours(2),
                ArrivalTime = DateTime.Now.AddHours(6),
                Price = 80.00m,
                IsAvailable = true
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
public void GetAllCars_ShouldReturnAllCars()
{
    // Act
    var result = _repository.GetAllCars().ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result.Count, Is.EqualTo(2), "The count of cars is incorrect."); // Use Count property
        Assert.That(result[0].OperatorName, Is.EqualTo("Operator 1"), "The first car's operator name is incorrect.");
    });
}

    [Test]
    public void AddCar_ShouldAddCar()
    {
        var car = new Car
        {
            Id = Guid.NewGuid(),
            OperatorName = "Operator 3",
            CarType = "Hatchback",
            From = "City E",
            To = "City F",
            DepartureTime = DateTime.Now.AddHours(3),
            ArrivalTime = DateTime.Now.AddHours(7),
            Price = 60.00m,
            IsAvailable = true
        };

        _repository.AddCar(car);
        _context.SaveChanges();

       var result = _repository.GetAllCars().ToList(); // Convert to a list for safe access

Assert.Multiple(() =>
{
    Assert.That(result.Count, Is.EqualTo(3), "The count of cars is incorrect."); // Use Count property
    Assert.That(result.Any(c => c.OperatorName == "Operator 3"), Is.True, "The expected car was not found.");
});
    }
}
