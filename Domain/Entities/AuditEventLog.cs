using System;

namespace Domain.Entities
{
    public class AuditEventLog
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int BatteryLevel { get; set; }
        public DateTime LogTime { get; set; }
    }
}
