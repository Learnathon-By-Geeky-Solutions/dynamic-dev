using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Test.InfrastructureTests.RepositoryTests
{
    [TestFixture]
    public class RepositoryTests
    {
        private ApplicationDbContext _context;
        private BusRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Repository")
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
public async Task AddAsync_ShouldAddEntity()
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
    await _repository.AddAsync(bus);
    await _context.SaveChangesAsync();
    var result = _repository.GetAll().ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result, Has.Count.EqualTo(3), "The count of buses is incorrect.");
        Assert.That(result.Any(b => b.OperatorName == "Operator 3"), Is.True, "The expected bus was not found.");
    });
}

        [Test]
public async Task RemoveAsync_ShouldRemoveEntity()
{
    // Arrange
    var busToRemove = _context.Buses.First();

    // Act
    await _repository.RemoveAsync(busToRemove.Id);
    await _context.SaveChangesAsync();
    var result = _repository.GetAll().ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result, Has.Count.EqualTo(3), "The count of buses is incorrect.");
        Assert.That(result.Any(b => b.OperatorName == "Operator 3"), Is.True, "The expected bus was not found.");
    });
}

        [Test]
        public async Task EditAsync_ShouldUpdateEntity()
        {
            var busToEdit = _context.Buses.First();
            busToEdit.OperatorName = "Updated Operator";

            await _repository.EditAsync(busToEdit);
            await _context.SaveChangesAsync();

            var updatedBus = _repository.GetById(busToEdit.Id);
            Assert.That(updatedBus.OperatorName, Is.EqualTo("Updated Operator"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntityById()
        {
            var busId = _context.Buses.First().Id;

            var result = await _repository.GetByIdAsync(busId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(busId));
        }

        [Test]
        public async Task GetCountAsync_ShouldReturnCorrectCount()
        {
            var count = await _repository.GetCountAsync();

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_ShouldReturnFilteredEntities()
        {
            // Act
            var result = await _repository.GetAsync(
                filter: b => b.BusType == "Luxury",
                orderBy: null,
                include: null,
                pageIndex: 1,
                pageSize: 10,
                isTrackingOff: false
            );

           // Assert
Assert.Multiple(() =>
{
    Assert.That(result.data, Has.Count.EqualTo(1), "The count of filtered buses is incorrect.");
    Assert.That(result.data[0].BusType, Is.EqualTo("Luxury"), "The bus type of the filtered result is incorrect.");
});
        }

    }
}