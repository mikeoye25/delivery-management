using Domain.Interfaces;
using Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliveryManagementDbContext Context;
        private IDroneRepository DroneRepository;
        private IMedicationRepository MedicationRepository;
        private IAuditEventLogRepository AuditEventLogRepository;

        public UnitOfWork(DeliveryManagementDbContext context)
        {
            this.Context = context;
        }
        public IDroneRepository Drones => DroneRepository = DroneRepository ?? new DroneRepository(Context);

        public IMedicationRepository Medications => MedicationRepository = MedicationRepository ?? new MedicationRepository(Context);

        public IAuditEventLogRepository AuditEventLogs => AuditEventLogRepository = AuditEventLogRepository ?? new AuditEventLogRepository(Context);

        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
