using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EasyTravel.Test.DomainTests.EntityTests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_CanBeInitialized_WithValidProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = "Male";
            var dateOfBirth = new DateTime(1990, 1, 1);
            var profilePicture = "profile.jpg";
            var createdAt = DateTime.UtcNow;

            // Act
            var user = new User
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                ProfilePicture = profilePicture,
                CreatedAt = createdAt,
                PhotographerBookings = new List<PhotographerBooking>(),
                Bookings = new List<Booking>()
            };

            // Assert
            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.FirstName, Is.EqualTo(firstName));
            Assert.That(user.LastName, Is.EqualTo(lastName));
            Assert.That(user.Gender, Is.EqualTo(gender));
            Assert.That(user.DateOfBirth, Is.EqualTo(dateOfBirth));
            Assert.That(user.ProfilePicture, Is.EqualTo(profilePicture));
            Assert.That(user.CreatedAt, Is.EqualTo(createdAt));
            Assert.That(user.PhotographerBookings, Is.Empty);
            Assert.That(user.Bookings, Is.Empty);
        }

        [Test]
        public void User_CanAddPhotographerBooking()
        {
            // Arrange
            var user = new User { PhotographerBookings = new List<PhotographerBooking>() };
            var booking = new PhotographerBooking
            {
                Id = Guid.NewGuid(),
                UserName = "John Doe",
                Email = "john.doe@example.com",
                Gender = "Male",
                EventType = "Wedding",
                EventLocation = "New York",
                EventDate = DateTime.UtcNow,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                TimeInHour = 2,
                PhotographerId = Guid.NewGuid()
            };

            // Act
            user.PhotographerBookings.Add(booking);

            // Assert
            Assert.That(user.PhotographerBookings, Has.Count.EqualTo(1));
            Assert.That(user.PhotographerBookings[0], Is.EqualTo(booking));
        }

        [Test]
        public void User_CanAddBooking()
        {
            // Arrange
            var user = new User { Bookings = new List<Booking>() };
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatus.Confirmed,
                BookingTypes = BookingTypes.Hotel,
                TotalAmount = 200.50m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            user.Bookings.Add(booking);

            // Assert
            Assert.That(user.Bookings, Has.Count.EqualTo(1));
            Assert.That(user.Bookings[0], Is.EqualTo(booking));
        }
    }
}
