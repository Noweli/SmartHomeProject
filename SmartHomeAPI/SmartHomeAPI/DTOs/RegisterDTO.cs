using System.ComponentModel.DataAnnotations;

namespace SmartHomeAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}