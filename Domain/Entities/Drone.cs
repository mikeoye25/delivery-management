using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Drone
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DroneModel Model { get; set; }
        public int WeightLimit { get; set; }
        public int BatteryCapacity { get; set; }
        public DroneState State { get; set; }
        public ICollection<Medication> Medications { get; set; }
    }
}
