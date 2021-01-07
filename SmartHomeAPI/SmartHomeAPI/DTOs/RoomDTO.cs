namespace SmartHomeAPI.DTOs
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public string SensorIp { get; set; }
        public string HeaterIp { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public int Interval { get; set; }
        public bool AutoHeatEnabled { get; set; }
    }
}