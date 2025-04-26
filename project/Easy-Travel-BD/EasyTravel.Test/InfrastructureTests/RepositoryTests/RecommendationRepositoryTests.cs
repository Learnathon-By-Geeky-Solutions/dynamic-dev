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
public class RecommendationRepositoryTests
{
    private ApplicationDbContext _context;
    private RecommendationRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Recommendation")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new RecommendationRepository(_context);

        // Seed data
        _context.Hotels.AddRange(new List<Hotel>
        {
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Grand Hotel",
                Address = "123 Main St",
                City = "City A",
                Phone = "123-456-7890",
                Email = "info@grandhotel.com",
                Rating = 5,
                Image = "hotel1.jpg",
                Description = "A luxurious hotel with modern amenities.",
                CreatedAt = DateTime.Now.AddDays(-10),
                UpdatedAt = DateTime.Now
            },
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Sunset Resort",
                Address = "456 Beach Rd",
                City = "City B",
                Phone = "987-654-3210",
                Email = "info@sunsetresort.com",
                Rating = 4,
                Image = "hotel2.jpg",
                Description = "A beautiful resort with stunning sunset views.",
                CreatedAt = DateTime.Now.AddDays(-20),
                UpdatedAt = DateTime.Now
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
public async Task GetRecommendationsAsync_ShouldReturnTopRatedHotels()
{
    // Act
    var result = (await _repository.GetRecommendationsAsync("hotels", 2)).ToList(); // Convert to a list for safe access

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(result.Count, Is.EqualTo(2), "The count of recommendations is incorrect."); // Use Count property
        Assert.That(result[0].Title, Is.EqualTo("Grand Hotel"), "The first recommendation's title is incorrect.");
    });
}
}

