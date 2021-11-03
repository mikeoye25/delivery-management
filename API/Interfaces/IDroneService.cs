using API.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IDroneService
    {
        Task<IEnumerable<Drone>> GetAllAsync();
        Task<IEnumerable<Drone>> GetAllWithMedicationsAsync();
        Task<Drone> GetBySerialNumber(string serialnumber);
        Task<RegisterDroneResponse> Register(RegisterDroneRequest registerDroneRequest);
        Task<LoadDroneResponse> LoadWithMedication(LoadDroneRequest loadDroneRequest);
        List<Medication> GetLoadedMedicationItems(Drone drone);
        Task<List<Drone>> GetAvailableForLoading();
        Task<DroneBatteryLevelResponse> GetBatteryLevel(string serialnumber);
        Task<AuditEventLog> CreateAuditEventLog(AuditEventLog auditeventlog);
        Task<IEnumerable<AuditEventLog>> GetAuditEventLogs();
        Task<IEnumerable<Medication>> GetMedications();
    }
}
