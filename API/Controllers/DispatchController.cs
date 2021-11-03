using API.DTOs;
using API.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly IDroneService DroneService;
        public DispatchController(IDroneService droneService)
        {
            DroneService = droneService;
        }

        [HttpGet("drones")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrones()
        {
            var drones = await DroneService.GetAllAsync();
            return Ok(drones);
        }

        [HttpPost("register-drone")]
        public async Task<ActionResult<RegisterDroneResponse>> Register(RegisterDroneRequest registerDroneRequest)
        {
            var newDrone = await DroneService.Register(registerDroneRequest);
            return Ok(newDrone);
        }

        [HttpPost("load-drone")]
        public async Task<ActionResult<LoadDroneResponse>> LoadDrone(LoadDroneRequest loadDroneRequest)
        {
            var newMedication = await DroneService.LoadWithMedication(loadDroneRequest);
            return Ok(newMedication);
        }

        [HttpGet("loaded-medications")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetLoadedMedications(string serialnumber)
        {
            var drone = await DroneService.GetBySerialNumber(serialnumber);
            var medications = new List<Medication>();
            if (drone != null)
            {
                medications = DroneService.GetLoadedMedicationItems(drone);
            }
            return Ok(medications);
        }

        [HttpGet("available-drones-for-loading")]
        public async Task<ActionResult<List<Drone>>> GetAvailableForLoading()
        {
            var drones = await DroneService.GetAvailableForLoading();
            return Ok(drones);
        }

        [HttpGet("drone-battery-level")]
        public async Task<ActionResult<DroneBatteryLevelResponse>> GetBatteryLevel(string serialnumber)
        {
            var batteryLevel = await DroneService.GetBatteryLevel(serialnumber);
            return Ok(batteryLevel);
        }

        [HttpGet("auditlogs")]
        public async Task<ActionResult<IEnumerable<AuditEventLog>>> GetAuditEventLogs()
        {
            var auditEventLogs = await DroneService.GetAuditEventLogs();
            return Ok(auditEventLogs);
        }

        [HttpGet("medications")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
        {
            var medications = await DroneService.GetMedications();
            return Ok(medications);
        }
    }
}
