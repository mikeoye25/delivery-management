namespace API.DTOs
{
    public class LoadDroneRequest
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int DroneId { get; set; }
    }
}
