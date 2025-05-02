using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Application.Services;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using EasyTravel.Domain.Services;


namespace EasyTravel.Test
{
    [TestFixture]
    public class BusServiceTests
    {
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<IBusRepository> _mockBusRepository;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IApplicationUnitOfWork>();
            _mockBusRepository = new Mock<IBusRepository>();

            _mockUnitOfWork.Setup(u => u.BusRepository).Returns(_mockBusRepository.Object);
        
        }

        // Other test methods remain unchanged
    }
}