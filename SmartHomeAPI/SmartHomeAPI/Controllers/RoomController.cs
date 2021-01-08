using System.Linq;
using System.Net.Http;
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
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly SensorRequestHelper _sensorRequestHelper = new SensorRequestHelper();

        public RoomController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("getRooms")]
        public ActionResult<RoomDTO[]> GetRooms()
        {
            RoomDTO[] rooms = _context.GetUserRooms(User.GetCurrentUser()).Select(r => r.ConverAppRoomToRoomDTO()).ToArray();

            return rooms;
        }
        
        [Authorize]
        [HttpPost("edit")]
        public async Task<ActionResult<RoomDTO>> EditRoom(AppRoom room)
        {
            if (string.IsNullOrEmpty(room.Name))
            {
                return BadRequest("Parameters provided incorrectly, missing name!");
            }
            
            await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(room.Id))}/setAutoHeater{_context.GetHeaterParmeters(room)}");

            if (_context.GetRoomBasedOnId(room.Id).AutoHeatEnabled && !room.AutoHeatEnabled)
            {
                await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(room.Id))}/turnOffAutoHeater");
            }

            if (!_context.GetRoomBasedOnId(room.Id).AutoHeatEnabled && room.AutoHeatEnabled)
            {
                await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(room.Id))}/turnOnAutoHeater");
            }
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return room.ConverAppRoomToRoomDTO();
        }
        
        [Authorize]
        [HttpPost("delete")]
        public async Task<ActionResult> DeleteRoom(AppRoom room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok($"Removed room with id {room.Id}");
        }
        
        [Authorize]
        [HttpGet("setHeater")]
        public async Task<ActionResult<RoomDTO>> SetHeaterConfiguration(string roomName, int? minTemperature, int? maxTemperature)
        {
            if (string.IsNullOrEmpty(roomName) || minTemperature == null || maxTemperature == null)
            {
                return BadRequest("Parameters provided incorrectly! name=[roomName]&minTemperature=[number]&maxTemperature=[number]");
            }
            
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), roomName);
            
            if (roomId == -1)
            {
                return BadRequest("Room with such name already exists!");
            }

            var room = _context.GetRoomBasedOnId(roomId);
            room.MaxTemp = maxTemperature.Value;
            room.MinTemp = minTemperature.Value;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return room.ConverAppRoomToRoomDTO();
        }
        
        [Authorize]
        [HttpGet("enableAutoHeater")]
        public async Task<ActionResult<RoomDTO>> EnableAutoHeater(string roomName, bool enabled)
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

            if (enabled)
            {
                var response =
                    await _httpClient.GetAsync(
                        $"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(roomId))}/turnOnAutoHeater");
                
                if (response.Content.ReadAsStringAsync().Result.Contains("Min and max temperature is not set!"))
                {
                    if (room.MinTemp == 0 && room.MaxTemp == 0)
                    {
                        return Problem("Min and max temp not set!");
                    }
                    await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(roomId))}/setAutoHeater{_context.GetHeaterParmeters(room)}");
                    response = await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(roomId))}/turnOnAutoHeater");
                }
                
                if (!response.IsSuccessStatusCode)
                {
                    return Problem($"Could not turn on auto heater on room sensors module! {response}");
                }
            }
            else
            {
                await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(_context.GetRoomSensorIp(roomId))}/turnOffAutoHeater");
            }
            
            room.AutoHeatEnabled = enabled;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return room.ConverAppRoomToRoomDTO();
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
                SensorsIP = roomDto.SensorsIp,
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