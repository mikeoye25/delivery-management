using API.DTOs;
using API.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class DispatchControllerTest
    {
        private readonly IDroneService Service;

        public DispatchControllerTest()
        {
            Service = new DroneServiceFake();
        }

        [Fact]
        public async void GetDrones_WhenCalled_ReturnsAllDrones()
        {
            var listDrones = await Service.GetAllAsync();
            var drones = Assert.IsType<List<Drone>>(listDrones);
            Assert.Equal(4, drones.Count);
        }

        [Fact]
        public async void RegisterDrone_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var testRegisterDroneRequest = new RegisterDroneRequest
            {
                Model = DroneModel.Cruiserweight,
                WeightLimit = 400
            };
            var createdResponse = await Service.Register(testRegisterDroneRequest);
            Assert.IsType<RegisterDroneResponse>(createdResponse);
            Assert.True(createdResponse.Succeeded);
        }

        [Fact]
        public async void LoadDrone_ValidObjectPassed_ReturnedCreatedResponse()
        {
            var testLoadDroneRequest = new LoadDroneRequest
            {
                Code = "3M_",
                DroneId = 1,
                Name = "3nM-_",
                Image = "",
                Weight = 40
            };
            var createdResponse = await Service.LoadWithMedication(testLoadDroneRequest);
            Assert.IsType<LoadDroneResponse>(createdResponse);
            Assert.True(createdResponse.Succeeded);
        }

        [Fact]
        public async void GetLoadedMedications_ValidObjectPassed_ReturnsLoadedMedicationItems()
        {
            var serialnumber = "1QWERTY";
            var drone = await Service.GetBySerialNumber(serialnumber);
            var listMedications = Service.GetLoadedMedicationItems(drone);
            var medications = Assert.IsType<List<Medication>>(listMedications);
            Assert.Equal(1, medications.Count);
        }

        [Fact]
        public async void GetAvailableDrones_ValidObjectPassed_ReturnsAvailableDrones()
        {
            var listDrones = await Service.GetAvailableForLoading();
            var drones = Assert.IsType<List<Drone>>(listDrones);
            Assert.Equal(4, drones.Count);
        }

        [Fact]
        public async void GetDroneBateryLevel_ValidObjectPassed_ReturnsDroneBateryLevel()
        {
            var serialnumber = "1QWERTY";
            var droneBatteryLevelResponse = await Service.GetBatteryLevel(serialnumber);
            Assert.Equal(100, droneBatteryLevelResponse.BatteryLevel);
        }
    }
}
