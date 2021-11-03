using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DroneRepository : Repository<Drone>, IDroneRepository
    {
        private DeliveryManagementDbContext DeliveryManagementDbContext
        {
            get { return Context as DeliveryManagementDbContext; }
        }
        public DroneRepository(DeliveryManagementDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Drone>> GetAllWithMedicationsAsync()
        {
            return await DeliveryManagementDbContext.Drones
                .Include(a => a.Medications)
                .ToListAsync();
        }

        public Task<Drone> GetWithMedicationsByIdAsync(int id)
        {
            return DeliveryManagementDbContext.Drones
                .Include(a => a.Medications)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public Task<Drone> GetWithMedicationsBySerialNumberAsync(string serialnumber)
        {
            return DeliveryManagementDbContext.Drones
                .Include(a => a.Medications)
                .FirstOrDefaultAsync(a => a.SerialNumber == serialnumber);
        }

        public async Task<IEnumerable<Drone>> GetAvailableForLoading()
        {
            return await DeliveryManagementDbContext.Drones
                .Where(a => a.BatteryCapacity >= 25)
                .ToListAsync();
        }
    }
}
