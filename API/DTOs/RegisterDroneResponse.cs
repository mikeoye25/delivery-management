namespace API.DTOs
{
    public class RegisterDroneResponse
    {
        public string SerialNumber { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
