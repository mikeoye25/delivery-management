using API.DTOs;
using API.Helpers;
using API.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class DroneServiceFake : IDroneService
    {
        private readonly List<Drone> Drones;
        private readonly List<Medication> Medications;
        private readonly List<AuditEventLog> AuditEventLogs;

        public DroneServiceFake()
        {
            Drones = new List<Drone>
            {
                new Drone
                {
                    Id = 1,
                    BatteryCapacity = 100,
                    Model = DroneModel.Cruiserweight,
                    SerialNumber = "1QWERTY",
                    State = DroneState.IDLE,
                    WeightLimit = 100
                },
                new Drone
                {
                    Id = 2,
                    BatteryCapacity = 100,
                    Model = DroneModel.Heavyweight,
                    SerialNumber = "2ASDF",
                    State = DroneState.IDLE,
                    WeightLimit = 200
                },
                new Drone
                {
                    Id = 3,
                    BatteryCapacity = 100,
                    Model = DroneModel.Lightweight,
                    SerialNumber = "3GHJKL",
                    State = DroneState.IDLE,
                    WeightLimit = 300
                },
                new Drone
                {
                    Id = 4,
                    BatteryCapacity = 100,
                    Model = DroneModel.Middleweight,
                    SerialNumber = "4UIOP",
                    State = DroneState.IDLE,
                    WeightLimit = 400
                }
            };

            Medications = new List<Medication>
            {
                new Medication
                {
                    Code = "",
                    DroneId = 1,
                    Image = "",
                    Name = "MEDICATION ONE",
                    Weight = 50
                },
                new Medication
                {
                    Code = "",
                    DroneId = 2,
                    Image = "",
                    Name = "MEDICATION TWO",
                    Weight = 70
                }
            };

            AuditEventLogs = new List<AuditEventLog>();
        }

        public async Task<IEnumerable<Drone>> GetAllAsync()
        {
            return Drones;
        }

        public async Task<IEnumerable<Drone>> GetAllWithMedicationsAsync()
        {
            return Drones;
        }

        public async Task<Drone> GetWithMedicationsByIdAsync(int id)
        {
            return Drones.Where(a => a.Id == id).FirstOrDefault();
        }

        private static string RandomString()
        {
            var rand = new Random();
            var length = rand.Next(10, 101); //returns random number between 10-100
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[length];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        public async Task<RegisterDroneResponse> Register(RegisterDroneRequest registerDroneRequest)
        {
            var drone = new Drone
            {
                BatteryCapacity = 100,
                Model = registerDroneRequest.Model,
                SerialNumber = RandomString(),
                State = DroneState.IDLE,
                WeightLimit = registerDroneRequest.WeightLimit
            };
            drone.Id = Drones.Count + 1;
            Drones.Add(drone);
            return new RegisterDroneResponse
            {
                Message = "Drone was addedd successfully",
                SerialNumber = drone.SerialNumber,
                Succeeded = true
            };
        }

        private async Task<bool> FailsLoadingConditionsAsync(Medication medication)
        {
            var drone = await GetWithMedicationsByIdAsync(medication.DroneId);
            var weightLimit = drone.WeightLimit;
            var totalMedicationWeight = Medications.Where(m => m.DroneId == drone.Id).Sum(s => s.Weight);
            var hasLowBattery = drone.BatteryCapacity < 25;
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
                    medication.Id = Medications.Count + 1;
                    Medications.Add(medication);
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
            return Drones.FirstOrDefault(a => a.SerialNumber == serialnumber);
        }

        public List<Medication> GetLoadedMedicationItems(Drone drone)
        {
            return Medications.Where(a => a.DroneId == drone.Id).ToList();
        }

        public async Task<List<Drone>> GetAvailableForLoading()
        {
            return Drones.Where(a => a.BatteryCapacity >= 25).ToList();
        }

        public async Task<DroneBatteryLevelResponse> GetBatteryLevel(string serialnumber)
        {
            var response = new DroneBatteryLevelResponse
            {
                Succeeded = false,
                BatteryLevel = 0
            };
            var drone = await GetBySerialNumber(serialnumber);
            if (drone != null)
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
            auditeventlog.Id = Drones.Count + 1;
            AuditEventLogs.Add(auditeventlog);
            return auditeventlog;
        }

        public async Task<IEnumerable<AuditEventLog>> GetAuditEventLogs()
        {
            return AuditEventLogs;
        }

        public async Task<IEnumerable<Medication>> GetMedications()
        {
            return Medications;
        }
    }
}
