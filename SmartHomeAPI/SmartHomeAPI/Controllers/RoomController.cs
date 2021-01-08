using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("getRooms")]
        public ActionResult<RoomDTO[]> GetRooms()
        {
            RoomDTO[] rooms = _context.GetUserRooms(User.GetCurrentUser()).Select(r => new RoomDTO
            {
                Name = r.Name,
                HeaterIp = r.HeaterIP,
                SensorIp = r.SensorsIP,
                AutoHeatEnabled = r.AutoHeatEnabled,
                Interval = r.Interval,
                MaxTemp = r.MaxTemp,
                MinTemp = r.MinTemp,
                HeaterEnabled = r.HeaterEnabled
            }).ToArray();

            return rooms;
        }
        
        [Authorize]
        [HttpGet("setHeater")]
        public async Task<ActionResult<RoomDTO>> SetHeaterConfiguration(string roomName, int? minTemperature, int? maxTemperature, int? interval)
        {
            if (string.IsNullOrEmpty(roomName) || minTemperature == null || maxTemperature == null || interval == null)
            {
                return BadRequest("Parameters provided incorrectly! name=[roomName]&minTemperature=[number]&maxTemperature=[number]&interval=[number]");
            }
            
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), roomName);
            
            if (roomId == -1)
            {
                return BadRequest("Room with such name already exists!");
            }

            var room = _context.GetRoomBasedOnId(roomId);
            room.MaxTemp = maxTemperature.Value;
            room.MinTemp = minTemperature.Value;
            room.Interval = interval.Value;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return new RoomDTO
            {
                Name = room.Name,
                Interval = room.Interval,
                MaxTemp = room.MaxTemp,
                MinTemp = room.MinTemp,
                AutoHeatEnabled = room.AutoHeatEnabled,
                HeaterIp = room.HeaterIP,
                SensorIp = room.SensorsIP,
                HeaterEnabled = room.HeaterEnabled
            };
        }
        
        [Authorize]
        [HttpGet("enableAutoHeater")]
        public async Task<ActionResult<RoomDTO>> Add(string roomName, bool enabled)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return BadRequest("Parameters provided incorrectly! name=[roomName]");
            }
            
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), roomName);
            
            if (roomId == -1)
            {
                return BadRequest("Room with such name already exists!");
            }
            
            var room = _context.GetRoomBasedOnId(roomId);
            room.AutoHeatEnabled = enabled;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return new RoomDTO
            {
                Name = room.Name,
                Interval = room.Interval,
                MaxTemp = room.MaxTemp,
                MinTemp = room.MinTemp,
                AutoHeatEnabled = room.AutoHeatEnabled,
                HeaterIp = room.HeaterIP,
                SensorIp = room.SensorsIP,
                HeaterEnabled = room.HeaterEnabled
            };
        }
        
        [Authorize]
        [HttpPost("add")]
        public ActionResult<RoomDTO> Add(RoomDTO roomDto)
        {
            if (CheckIfRoomExists(roomDto.Name))
            {
                return BadRequest("Room with such name already exists!");
            }
            
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
        
        [Authorize]
        [HttpGet("checkHeater")]
        public async Task<ActionResult<bool>> CheckHeater(string roomName)
        {
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), roomName);
            var room = _context.GetRoomBasedOnId(roomId);

            if (room == null)
            {
                return BadRequest("Room with such name doesn't exists!");
            }

            return room.HeaterEnabled;
        }

        private bool CheckIfRoomExists(string roomName) => _context.Rooms
            .AnyAsync(r => r.Name == roomName && r.AppUser == User.GetCurrentUser()).Result;
    }
}