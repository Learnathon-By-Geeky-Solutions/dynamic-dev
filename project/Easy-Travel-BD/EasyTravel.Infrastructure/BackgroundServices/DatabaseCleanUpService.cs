using EasyTravel.Domain;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.BackgroundServices
{
    public class DatabaseCleanUpService : BackgroundService
    {
        private readonly ILogger<DatabaseCleanUpService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseCleanUpService(
            ILogger<DatabaseCleanUpService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CustomBackgroundService started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Add your background logic here
                    _logger.LogInformation("CustomBackgroundService running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Example: resolve a scoped service
                         var myService = scope.ServiceProvider.GetRequiredService<IApplicationUnitOfWork>();
                        await myService.BookingRepository.RemoveAsync(b =>
                        
                            b.BookingStatus == BookingStatus.Pending &&
                            b.CreatedAt < DateTime.UtcNow.AddMinutes(-15) &&
                            b.PhotographerBooking != null && b.PhotographerBooking.Id != b.Id ||
                            b.GuideBooking != null && b.GuideBooking.Id != b.Id ||
                            b.BusBooking != null && b.BusBooking.Id != b.Id ||
                            b.CarBooking != null && b.CarBooking.Id != b.Id ||
                            b.HotelBooking != null && b.HotelBooking.Id != b.Id
                        );
                    }

                    // Wait before the next execution
                    await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in CustomBackgroundService");
                }
            }

            _logger.LogInformation("CustomBackgroundService is stopping.");
        }
    }
}
