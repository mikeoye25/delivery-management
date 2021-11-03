using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        private DeliveryManagementDbContext DeliveryManagementDbContext
        {
            get { return Context as DeliveryManagementDbContext; }
        }
        public MedicationRepository(DeliveryManagementDbContext context)
            : base(context)
        { }
    }
}
