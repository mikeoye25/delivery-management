using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class DroneService : IDroneService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly AppSettings Settings;

        public DroneService(IUnitOfWork unitOfWork, AppSettings settings)
        {
            UnitOfWork = unitOfWork;
            Settings = settings;
        }

        public async Task<IEnumerable<Drone>> GetAllAsync()
        {
            return await UnitOfWork.Drones.GetAllAsync();
        }

        public async Task<IEnumerable<Drone>> GetAllWithMedicationsAsync()
        {
            return await UnitOfWork.Drones.GetAllWithMedicationsAsync();
        }

        public async Task<Drone> GetWithMedicationsByIdAsync(int id)
        {
            return await UnitOfWork.Drones.GetWithMedicationsByIdAsync(id);
        }

        public async Task<RegisterDroneResponse> Register(RegisterDroneRequest registerDroneRequest)
        {
            var response = new RegisterDroneResponse
            {
                Succeeded = false
            };
            var allDrones = await GetAllAsync();
            if (allDrones.ToList().Count >= Settings.MaximumDrone)
            {
                response.Message = $"Drones cannot exceed {Settings.MaximumDrone}";
                return response;
            }
            var drone = new Drone
            {
                BatteryCapacity = Settings.BatteryCapacity,
                Model = registerDroneRequest.Model,
                SerialNumber = Utilities.RandomString(),
                State = DroneState.IDLE,
                WeightLimit = registerDroneRequest.WeightLimit
            };
            await UnitOfWork.Drones.AddAsync(drone);
            response.Succeeded = true;
            response.Message = "Drone created successful";
            response.SerialNumber = drone.SerialNumber;
            return response;
        }

        private async Task<bool> FailsLoadingConditionsAsync(Medication medication)
        {
            var drone = await GetWithMedicationsByIdAsync(medication.DroneId);
            var weightLimit = drone.WeightLimit;
            var totalMedicationWeight = drone.Medications.Sum(s => s.Weight);
            var hasLowBattery = drone.BatteryCapacity < Settings.LowBatteryCapacity;
            var wouldExceedWeightLimit = (weightLimit - totalMedicationWeight) < medication.Weight;
            return hasLowBattery || wouldExceedWeightLimit;
        }

        public async Task<LoadDroneResponse> LoadWithMedication(LoadDroneRequest loadDroneRequest)
        {
            var response = new LoadDroneResponse
            {
                Succeeded = false,
                Code = loadDroneRequest.Code
            };
            if (Utilities.IsValidMedicationName(loadDroneRequest.Name) && Utilities.IsValidMedicationCode(loadDroneRequest.Code))
            {
                var medication = new Medication
                {
                    Code = loadDroneRequest.Code,
                    DroneId = loadDroneRequest.DroneId,
                    Image = loadDroneRequest.Image,
                    Name = loadDroneRequest.Name,
                    Weight = loadDroneRequest.Weight
                };
                var failsLoadingConditions = await FailsLoadingConditionsAsync(medication);
                if (!failsLoadingConditions)
                {
                    await UnitOfWork.Medications.AddAsync(medication);
                    response.Message = "Drone loaded successful";
                    response.Succeeded = true;
                    return response;
                }
                response.Message = "Fails to meet loading conditions";
                return response;
            }
            response.Message = "Invalid input";
            return response;
        }

        public async Task<Drone> GetBySerialNumber(string serialnumber)
        {
            return await UnitOfWork.Drones.GetWithMedicationsBySerialNumberAsync(serialnumber);
        }

        public List<Medication> GetLoadedMedicationItems(Drone drone)
        {
            return drone.Medications.ToList();
        }

        public async Task<List<Drone>> GetAvailableForLoading()
        {
            var drones = await UnitOfWork.Drones.GetAvailableForLoading();
            return drones.ToList();
        }

        public async Task<DroneBatteryLevelResponse> GetBatteryLevel(string serialnumber)
        {
            var response = new DroneBatteryLevelResponse
            {
                Succeeded = false,
                BatteryLevel = 0
            };
            var drone = await GetBySerialNumber(serialnumber);
            if(drone != null)
            {
                response.Succeeded = true;
                response.Message = "success";
                response.BatteryLevel = drone.BatteryCapacity;
                return response;
            }
            response.Message = "Drone not found";
            return response;
        }

        public async Task<AuditEventLog> CreateAuditEventLog(AuditEventLog auditeventlog)
        {
            await UnitOfWork.AuditEventLogs.AddAsync(auditeventlog);
            return auditeventlog;
        }

        public async Task<IEnumerable<AuditEventLog>> GetAuditEventLogs()
        {
            return await UnitOfWork.AuditEventLogs.GetAllAsync();
        }

        public async Task<IEnumerable<Medication>> GetMedications()
        {
            return await UnitOfWork.Medications.GetAllAsync();
        }
    }
}
