using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AuditEventLogRepository : Repository<AuditEventLog>, IAuditEventLogRepository
    {
        private DeliveryManagementDbContext DeliveryManagementDbContext
        {
            get { return Context as DeliveryManagementDbContext; }
        }
        public AuditEventLogRepository(DeliveryManagementDbContext context)
            : base(context)
        { }
    }
}
