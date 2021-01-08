namespace SmartHomeAPI.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string AppUser { get; set; }
        public string Name { get; set; }
        public string SensorsIp { get; set; }
        public string HeaterIp { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public int Interval { get; set; }
        public bool AutoHeatEnabled { get; set; }
        public bool HeaterEnabled { get; set; }
    }
}