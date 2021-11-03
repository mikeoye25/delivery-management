using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DeliveryManagementDbContext : DbContext
    {
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<AuditEventLog> AuditEventLogs { get; set; }

        public DeliveryManagementDbContext(DbContextOptions<DeliveryManagementDbContext> options) : base(options)
        {
        }
    }
}
