using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDroneRequest
    {
        [Range(0, 500)]
        public int WeightLimit { get; set; }
        public DroneModel Model { get; set; }
    }
}
