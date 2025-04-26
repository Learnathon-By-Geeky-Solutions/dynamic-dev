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
    public class PhotographerRepositoryTests
    {
        private ApplicationDbContext _context;
        private PhotographerRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Photographer")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PhotographerRepository(_context);

            // Seed data
            var agency = new Agency
            {
                Id = Guid.NewGuid(),
                Name = "Creative Agency",
                Address = "123 Main St",
                ContactNumber = "123-456-7890",
                LicenseNumber = "LICENSE123",
                AddDate = DateTime.Now.AddYears(-5)
            };

            _context.Agencies.Add(agency);

            _context.Photographers.AddRange(new List<Photographer>
            {
                new Photographer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    ContactNumber = "123-456-7890",
                    PreferredLocations = "City A, City B",
                    PreferredEvents = "Weddings, Portraits",
                    Address = "123 Main St",
                    ProfilePicture = "profile1.jpg",
                    Bio = "Experienced photographer specializing in weddings.",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    Skills = "Photography, Editing",
                    PortfolioUrl = "http://portfolio.com/johndoe",
                    Specialization = "Weddings",
                    YearsOfExperience = 10,
                    Availability = true,
                    HourlyRate = 50.00m,
                    Rating = 4.5m,
                    HireDate = DateTime.Now.AddYears(-3),
                    UpdatedAt = DateTime.Now,
                    AgencyId = agency.Id
                },
                new Photographer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    ContactNumber = "987-654-3210",
                    PreferredLocations = "City C, City D",
                    PreferredEvents = "Nature, Wildlife",
                    Address = "456 Elm St",
                    ProfilePicture = "profile2.jpg",
                    Bio = "Nature photographer with a passion for wildlife.",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Skills = "Photography, Editing",
                    PortfolioUrl = "http://portfolio.com/janesmith",
                    Specialization = "Nature",
                    YearsOfExperience = 5,
                    Availability = true,
                    HourlyRate = 40.00m,
                    Rating = 4.8m,
                    HireDate = DateTime.Now.AddYears(-2),
                    UpdatedAt = DateTime.Now,
                    AgencyId = agency.Id
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
public void GetAllPhotographers_ShouldReturnAllPhotographers()
{
    // Act
    var result = _repository.GetAll().ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result.Count, Is.EqualTo(2), "The count of photographers is incorrect."); // Use Count property
        Assert.That(result[0].FirstName, Is.EqualTo("John"), "The first photographer's first name is incorrect.");
    });
}

        [Test]
        public void AddPhotographer_ShouldAddPhotographer()
        {
            // Arrange
            var photographer = new Photographer
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                ContactNumber = "555-123-4567",
                PreferredLocations = "City E, City F",
                PreferredEvents = "Portraits, Events",
                Address = "789 Oak St",
                ProfilePicture = "profile3.jpg",
                Bio = "Portrait photographer with a creative touch.",
                DateOfBirth = new DateTime(1995, 3, 10),
                Skills = "Photography, Editing",
                PortfolioUrl = "http://portfolio.com/alicejohnson",
                Specialization = "Portraits",
                YearsOfExperience = 7,
                Availability = true,
                HourlyRate = 60.00m,
                Rating = 4.9m,
                HireDate = DateTime.Now.AddYears(-1),
                UpdatedAt = DateTime.Now,
                AgencyId = _context.Agencies.First().Id
            };

            // Act
            _repository.Add(photographer);
            _context.SaveChanges();

           // Assert
var result = _repository.GetAll().ToList(); // Convert to a list for safe access

Assert.Multiple(() =>
{
    Assert.That(result.Count, Is.EqualTo(3), "The count of photographers is incorrect."); // Use Count property
    Assert.That(result.Any(p => p.FirstName == "Alice"), Is.True, "The expected photographer 'Alice' was not found.");
});
        }
    }
}