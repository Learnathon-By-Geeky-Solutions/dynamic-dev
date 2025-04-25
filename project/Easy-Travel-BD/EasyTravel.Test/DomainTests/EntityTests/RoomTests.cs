using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace UnitTest.Entities
{
    [TestFixture]
    public class RoomTests
    {
        [Test]
        public void Room_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var room = new Room
            {
                Id = Guid.NewGuid(),
                RoomNumber = "101",
                RoomType = "Deluxe",
                PricePerNight = 150.00m,
                MaxOccupancy = 2,
                Description = "A deluxe room with a king-size bed.",
                IsAvailable = true
            };

            // Assert
            Assert.That(room.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(room.RoomNumber, Is.EqualTo("101"));
            Assert.That(room.RoomType, Is.EqualTo("Deluxe"));
            Assert.That(room.PricePerNight, Is.EqualTo(150.00m));
            Assert.That(room.MaxOccupancy, Is.EqualTo(2));
            Assert.That(room.Description, Is.EqualTo("A deluxe room with a king-size bed."));
            Assert.That(room.IsAvailable, Is.True);
        }
    }
}
