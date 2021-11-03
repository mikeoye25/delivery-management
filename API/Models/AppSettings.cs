namespace API.Models
{
    public class AppSettings
    {
        public int BatteryCapacity { get; set; }
        public int LowBatteryCapacity { get; set; }
        public int AuditEventLogPeriod { get; set; }
        public int MaximumDrone { get; set; }
    }
}
