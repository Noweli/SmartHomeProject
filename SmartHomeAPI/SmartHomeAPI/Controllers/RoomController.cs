using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAPI.Data;
using SmartHomeAPI.DTOs;
using SmartHomeAPI.Entity;
using SmartHomeAPI.Helpers;

namespace SmartHomeAPI.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly DataContext _context;

        public RoomController(DataContext context)
        {
            _context = context;
        }
        
        [Authorize]
        [HttpPost("add")]
        public ActionResult<RoomDTO> Add(RoomDTO roomDto)
        {
            var room = new AppRoom
            {
                Name = roomDto.Name,
                AppUser = User.GetCurrentUser(),
                SensorsIP = roomDto.SensorIp,
                HeaterIP = roomDto.HeaterIp
            };

            _context.Rooms.Add(room);
            _context.SaveChangesAsync();

            return roomDto;
        }
    }
}