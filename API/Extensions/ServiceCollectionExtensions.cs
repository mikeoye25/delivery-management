using API.Interfaces;
using API.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IAuditEventLogRepository, AuditEventLogRepository>()
                .AddScoped<IDroneRepository, DroneRepository>()
                .AddScoped<IMedicationRepository, MedicationRepository>();
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<DeliveryManagementDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "DeliveryManagementDb"));
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services.AddScoped<IDroneService, DroneService>();
        }
    }
}
