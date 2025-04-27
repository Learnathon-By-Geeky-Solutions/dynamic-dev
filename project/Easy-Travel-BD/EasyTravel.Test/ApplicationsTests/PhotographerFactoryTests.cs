using EasyTravel.Application.Factories;
using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace EasyTravel.Test.ApplicationsTests
{
    /// <summary>
    /// Unit tests for the PhotographerFactory class.
    /// </summary>
    /// <remarks>
    /// This class contains tests to ensure that the PhotographerFactory creates instances of the Photographer class
    /// with the expected default values.
    /// </remarks>
    [TestFixture]
    public class PhotographerFactoryTests
    {
        private PhotographerFactory _photographerFactory;

        [SetUp]
        public void SetUp()
        {
            _photographerFactory = new PhotographerFactory();
        }

        [Test]
        public void CreateInstance_ShouldReturnPhotographerWithDefaultValues()
        {
            // Act
            var photographer = _photographerFactory.CreateInstance();

            // Assert
            Assert.Multiple(() =>
            {

                Assert.That(photographer, Is.Not.Null, "Photographer instance should not be null.");
                Assert.That(photographer.FirstName, Is.EqualTo(string.Empty),
                    "FirstName should be initialized to an empty string.");
                Assert.That(photographer.LastName, Is.EqualTo(string.Empty),
                    "LastName should be initialized to an empty string.");
                Assert.That(photographer.Email, Is.EqualTo(string.Empty),
                    "Email should be initialized to an empty string.");
                Assert.That(photographer.ContactNumber, Is.EqualTo(string.Empty),
                    "ContactNumber should be initialized to an empty string.");
                Assert.That(photographer.PreferredLocations, Is.EqualTo(string.Empty),
                    "PreferredLocations should be initialized to an empty string.");
                Assert.That(photographer.PreferredEvents, Is.EqualTo(string.Empty),
                    "PreferredEvents should be initialized to an empty string.");
                Assert.That(photographer.Address, Is.EqualTo(string.Empty),
                    "Address should be initialized to an empty string.");
                Assert.That(photographer.ProfilePicture, Is.EqualTo(string.Empty),
                    "ProfilePicture should be initialized to an empty string.");
                Assert.That(photographer.Bio, Is.EqualTo(string.Empty),
                    "Bio should be initialized to an empty string.");
                Assert.That(photographer.DateOfBirth, Is.EqualTo(DateTime.MinValue),
                    "DateOfBirth should be initialized to DateTime.MinValue.");
                Assert.That(photographer.Skills, Is.EqualTo(string.Empty),
                    "Skills should be initialized to an empty string.");
                Assert.That(photographer.PortfolioUrl, Is.EqualTo(string.Empty),
                    "PortfolioUrl should be initialized to an empty string.");
                Assert.That(photographer.Specialization, Is.EqualTo(string.Empty),
                    "Specialization should be initialized to an empty string.");
                Assert.That(photographer.YearsOfExperience, Is.EqualTo(0),
                    "YearsOfExperience should be initialized to 0.");
                Assert.That(photographer.Availability, Is.False, "Availability should be initialized to false.");
                Assert.That(photographer.HourlyRate, Is.EqualTo(0), "HourlyRate should be initialized to 0.");
                Assert.That(photographer.Rating, Is.EqualTo(0), "Rating should be initialized to 0.");
                Assert.That(photographer.SocialMediaLinks, Is.Null, "SocialMediaLinks should be initialized to null.");
                Assert.That(photographer.Status, Is.Null, "Status should be initialized to null.");
                Assert.That(photographer.AgencyId, Is.Not.EqualTo(Guid.Empty),
                    "AgencyId should be initialized to a new Guid.");
            });
        }
    }

}

