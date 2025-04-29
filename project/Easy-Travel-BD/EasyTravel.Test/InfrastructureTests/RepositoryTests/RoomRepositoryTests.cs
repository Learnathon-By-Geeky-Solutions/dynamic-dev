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
    public class RoomRepositoryTests
    {
        private ApplicationDbContext _context;
        private RoomRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Room")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RoomRepository(_context);

            // Seed data
            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Grand Hotel",
                Address = "123 Main St",
                City = "City A",
                Phone = "123-456-7890",
                Email = "info@grandhotel.com",
                Rating = 5,
                Image = "hotel.jpg",
                Description = "A luxurious hotel with modern amenities.",
                CreatedAt = DateTime.Now.AddDays(-10),
                UpdatedAt = DateTime.Now
            };

            _context.Hotels.Add(hotel);

            _context.Rooms.AddRange(new List<Room>
            {
                new Room
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = "101",
                    RoomType = "Deluxe",
                    PricePerNight = 100.00m,
                    MaxOccupancy = 2,
                    Description = "A deluxe room with a king-size bed.",
                    IsAvailable = true,
                    HotelId = hotel.Id
                },
                new Room
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = "102",
                    RoomType = "Standard",
                    PricePerNight = 80.00m,
                    MaxOccupancy = 2,
                    Description = "A standard room with a queen-size bed.",
                    IsAvailable = false,
                    HotelId = hotel.Id
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
public void GetRooms_ShouldReturnRoomsForHotel()
{
    // Arrange
    var hotelId = _context.Hotels.First().Id;

    // Act
    var result = _repository.GetRooms(hotelId).ToList(); // Convert to a list for safe access

   // Assert
Assert.Multiple(() =>
{
    Assert.That(result, Has.Count.EqualTo(2), "The count of rooms is incorrect."); // Use Has.Count.EqualTo
    Assert.That(result[0].RoomNumber, Is.EqualTo("101"), "The first room's number is incorrect.");
});
}
    }

}