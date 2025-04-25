using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace UnitTest.Entities
{
    [TestFixture]
    public class CarBookingTests
    {
        [Test]
        public void CarBooking_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var booking = new CarBooking
            {
                Id = Guid.NewGuid(),
                CarId = Guid.NewGuid(),
                PassengerName = "Alice Johnson",
                Email = "alice.johnson@example.com",
                PhoneNumber = "987-654-3210",
                BookingDate = DateTime.UtcNow
            };

            // Assert
            Assert.That(booking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(booking.CarId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(booking.PassengerName, Is.EqualTo("Alice Johnson"));
            Assert.That(booking.Email, Is.EqualTo("alice.johnson@example.com"));
            Assert.That(booking.PhoneNumber, Is.EqualTo("987-654-3210"));
            Assert.That(booking.BookingDate, Is.Not.EqualTo(default(DateTime)));
        }
    }
}
