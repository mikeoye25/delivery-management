using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDroneRepository Drones { get; }
        IMedicationRepository Medications { get; }
        IAuditEventLogRepository AuditEventLogs { get; }
        Task<int> CommitAsync();
    }
}
