using System.ComponentModel.DataAnnotations;

namespace SmartHomeAPI.Entity
{
    public class AppRoom
    {
        public int Id { get; set; }
        [Required]
        public string AppUser { get; set; }
        [Required]
        public string Name { get; set; }
        public string SensorsIP { get; set; }
        public string HeaterIP { get; set;}
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public int Interval { get; set; }
        public bool AutoHeatEnabled { get; set; }
        
    }
}