using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace UnitTest.Entities
{
    [TestFixture]
    public class HotelBookingTests
    {
        [Test]
        public void HotelBooking_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var booking = new HotelBooking
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.NewGuid(),
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(2),
                RoomIdsJson = "[\"101\", \"102\"]"
            };

            // Assert
            Assert.That(booking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(booking.HotelId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(booking.CheckInDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(booking.CheckOutDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(booking.RoomIdsJson, Is.EqualTo("[\"101\", \"102\"]"));
        }
    }
}
