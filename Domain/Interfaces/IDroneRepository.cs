using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDroneRepository : IRepository<Drone>
    {
        Task<IEnumerable<Drone>> GetAllWithMedicationsAsync();
        Task<Drone> GetWithMedicationsByIdAsync(int id);
        Task<Drone> GetWithMedicationsBySerialNumberAsync(string serialnumber);
        Task<IEnumerable<Drone>> GetAvailableForLoading();
    }
}
