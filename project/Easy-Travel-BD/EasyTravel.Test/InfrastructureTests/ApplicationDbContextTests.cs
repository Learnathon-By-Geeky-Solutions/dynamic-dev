using EasyTravel.Infrastructure.Data;
using EasyTravel.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace EasyTravel.Test.InfrastructureTests;
[TestFixture]
public class ApplicationDbContextTests
{
    private ApplicationDbContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_ApplicationDbContext")
            .Options;

        _context = new ApplicationDbContext(options);

        // Seed data
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
public void ShouldSeedDefaultRoles()
{
    // Act
    var roles = _context.Roles.ToList();

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(roles.Count, Is.EqualTo(6), "The count of roles is incorrect.");
        Assert.That(roles.Any(r => r.Name == "admin"), Is.True, "The 'admin' role is missing.");
        Assert.That(roles.Any(r => r.Name == "client"), Is.True, "The 'client' role is missing.");
        Assert.That(roles.Any(r => r.Name == "agencyManager"), Is.True, "The 'agencyManager' role is missing.");
        Assert.That(roles.Any(r => r.Name == "busManager"), Is.True, "The 'busManager' role is missing.");
        Assert.That(roles.Any(r => r.Name == "carManager"), Is.True, "The 'carManager' role is missing.");
        Assert.That(roles.Any(r => r.Name == "hotelManager"), Is.True, "The 'hotelManager' role is missing.");
    });
}

  [Test]
public void ShouldSeedDefaultAgencies()
{
    // Act
    var agencies = _context.Agencies.ToList();

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(agencies.Count, Is.EqualTo(1), "The count of agencies is incorrect.");
        Assert.That(agencies[0].Name, Is.EqualTo("Irfan"), "The agency name is incorrect.");
    });
}

   [Test]
public void ShouldSeedDefaultPhotographers()
{
    // Act
    var photographers = _context.Photographers.ToList();

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(photographers.Count, Is.EqualTo(1), "The count of photographers is incorrect.");
        Assert.That(photographers[0].FirstName, Is.EqualTo("Irfan"), "The photographer's first name is incorrect.");
    });
}
[Test]
public void ShouldSeedDefaultGuides()
{
    // Act
    var guides = _context.Guides.ToList();

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(guides.Count, Is.EqualTo(1), "The count of guides is incorrect.");
        Assert.That(guides[0].FirstName, Is.EqualTo("Irfan"), "The guide's first name is incorrect.");
    });
}

   [Test]
public void ShouldSeedDefaultHotels()
{
    // Act
    var hotels = _context.Hotels.ToList();

    // Assert
    Assert.Multiple(() =>
    {
        Assert.That(hotels.Count, Is.EqualTo(2), "The count of hotels is incorrect.");
        Assert.That(hotels[0].Name, Is.EqualTo("Grand Hotel"), "The first hotel's name is incorrect.");
        Assert.That(hotels[1].Name, Is.EqualTo("Sunset Resort"), "The second hotel's name is incorrect.");
    });
}


}

