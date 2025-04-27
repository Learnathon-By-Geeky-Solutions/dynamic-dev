using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EasyTravel.Test.DomainTests.EntityTests
{
    [TestFixture]
    public class GuideTests
    {
        [Test]
        public void Guide_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var guide = new Guide
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                ContactNumber = "987-654-3210",
                Address = "456 Elm St, Los Angeles, CA",
                ProfilePicture = "profile.jpg",
                Bio = "Experienced tour guide specializing in historical tours.",
                DateOfBirth = new DateTime(1985, 5, 15),
                LanguagesSpoken = "English, Spanish",
                PreferredLocations = "Los Angeles, San Francisco",
                PreferredEvents = "Historical Tours, City Tours",
                Specialization = "Historical Tours",
                YearsOfExperience = 15,
                LicenseNumber = "GUIDE12345",
                Availability = true,
                HourlyRate = 75.00m,
                Rating = 4.9m,
                HireDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Active",
                AgencyId = Guid.NewGuid(),
                Agencies = new List<Agency>(),
                GuideBookings = new List<GuideBooking>()
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(guide.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(guide.FirstName, Is.EqualTo("Alice"));
                Assert.That(guide.LastName, Is.EqualTo("Smith"));
                Assert.That(guide.Email, Is.EqualTo("alice.smith@example.com"));
                Assert.That(guide.ContactNumber, Is.EqualTo("987-654-3210"));
                Assert.That(guide.Address, Is.EqualTo("456 Elm St, Los Angeles, CA"));
                Assert.That(guide.ProfilePicture, Is.EqualTo("profile.jpg"));
                Assert.That(guide.Bio, Is.EqualTo("Experienced tour guide specializing in historical tours."));
                Assert.That(guide.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 15)));
                Assert.That(guide.LanguagesSpoken, Is.EqualTo("English, Spanish"));
                Assert.That(guide.PreferredLocations, Is.EqualTo("Los Angeles, San Francisco"));
                Assert.That(guide.PreferredEvents, Is.EqualTo("Historical Tours, City Tours"));
                Assert.That(guide.Specialization, Is.EqualTo("Historical Tours"));
                Assert.That(guide.YearsOfExperience, Is.EqualTo(15));
                Assert.That(guide.LicenseNumber, Is.EqualTo("GUIDE12345"));
                Assert.That(guide.Availability, Is.True);
                Assert.That(guide.HourlyRate, Is.EqualTo(75.00m));
                Assert.That(guide.Rating, Is.EqualTo(4.9m));
                Assert.That(guide.HireDate, Is.Not.EqualTo(default(DateTime)));
                Assert.That(guide.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
                Assert.That(guide.Status, Is.EqualTo("Active"));
                Assert.That(guide.AgencyId, Is.Not.EqualTo(Guid.Empty));
                Assert.That(guide.Agencies, Is.Empty);
                Assert.That(guide.GuideBookings, Is.Empty);
            });
        }

        [Test]
        public void Guide_CanAddGuideBooking()
        {
            // Arrange
            var guide = new Guide
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                ContactNumber = "987-654-3210",
                Address = "456 Elm St, Los Angeles, CA",
                ProfilePicture = "profile.jpg",
                Bio = "Experienced tour guide specializing in historical tours.",
                DateOfBirth = new DateTime(1985, 5, 15),
                LanguagesSpoken = "English, Spanish",
                PreferredLocations = "Los Angeles, San Francisco",
                PreferredEvents = "Historical Tours, City Tours",
                Specialization = "Historical Tours",
                YearsOfExperience = 15,
                LicenseNumber = "GUIDE12345",
                Availability = true,
                HourlyRate = 75.00m,
                Rating = 4.9m,
                HireDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Active",
                AgencyId = Guid.NewGuid(),
                Agencies = new List<Agency>(),
                GuideBookings = new List<GuideBooking>()
            };

            var booking = new GuideBooking
            {
                Id = Guid.NewGuid(),
                UserName = "Bob Johnson",
                Email = "bob.johnson@example.com",
                Gender = "Male",
                EventType = "City Tour",
                EventLocation = "San Francisco",
                EventDate = DateTime.UtcNow,
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(12),
                TimeInHour = 3,
                GuideId = guide.Id
            };

            // Act
            guide.GuideBookings.Add(booking);

            // Assert
            Assert.That(guide.GuideBookings, Has.Count.EqualTo(1));
            Assert.That(guide.GuideBookings[0], Is.EqualTo(booking));
        }
    }
}
