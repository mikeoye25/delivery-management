namespace API.DTOs
{
    public class DroneBatteryLevelResponse
    {
        public int BatteryLevel { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
