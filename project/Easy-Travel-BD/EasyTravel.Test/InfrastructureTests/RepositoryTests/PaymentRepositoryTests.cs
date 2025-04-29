using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class PaymentRepositoryTests
{
    private ApplicationDbContext _context;
    private PaymentRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Payment")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new PaymentRepository(_context);

        // Seed data
        _context.Payments.AddRange(new List<Payment>
        {
            new Payment
            {
                Id = Guid.NewGuid(),
                PaymentMethod = PaymentMethods.SSLCommerz,
                Amount = 100.00m,
                PaymentDate = DateTime.Now.AddDays(-1),
                PaymentStatus = PaymentStatus.Completed,
                BookingId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid()
            },
            new Payment
            {
                Id = Guid.NewGuid(),
                PaymentMethod = PaymentMethods.SSLCommerz,
                Amount = 200.00m,
                PaymentDate = DateTime.Now.AddDays(-2),
                PaymentStatus = PaymentStatus.Refunded,
                BookingId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid()
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
    public void GetAllPayments_ShouldReturnAllPayments()
    {
        var result = _repository.GetAll();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().PaymentMethod, Is.EqualTo(PaymentMethods.SSLCommerz));
    }

    [Test]
    public void AddPayment_ShouldAddPayment()
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            PaymentMethod = PaymentMethods.SSLCommerz,
            Amount = 300.00m,
            PaymentDate = DateTime.Now,
            PaymentStatus = PaymentStatus.Refunded,
            BookingId = Guid.NewGuid(),
            TransactionId = Guid.NewGuid()
        };

        _repository.Add(payment);
        _context.SaveChanges();

        var result = _repository.GetAll();
        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(result.Any(p => p.PaymentMethod == PaymentMethods.SSLCommerz), Is.True);
    }
}
