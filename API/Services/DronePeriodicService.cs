using API.Interfaces;
using API.Models;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class DronePeriodicService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DronePeriodicService> _logger;
        private Timer _timer = null!;
        private readonly AppSettings Settings;

        public DronePeriodicService(IServiceProvider serviceProvider, ILogger<DronePeriodicService> logger, AppSettings settings)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            Settings = settings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(
                CheckAndLogDronesBatteryLevels,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(Settings.AuditEventLogPeriod)
            );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void CheckAndLogDronesBatteryLevels(object? state)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IDroneService scopedDroneService = scope.ServiceProvider.GetRequiredService<IDroneService>();
                var drones = await scopedDroneService.GetAllAsync();
                foreach (var drone in drones)
                {
                    var auditEventLog = new AuditEventLog
                    {
                        SerialNumber = drone.SerialNumber,
                        BatteryLevel = drone.BatteryCapacity,
                        LogTime = DateTime.Now
                    };
                    await scopedDroneService.CreateAuditEventLog(auditEventLog);
                }
            }
            _logger.LogInformation("Worker ends at: {time}", DateTimeOffset.Now);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
