using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.Data;
using SmartHomeAPI.DTOs;
using SmartHomeAPI.Enums;
using SmartHomeAPI.Helpers;

namespace SmartHomeAPI.Controllers
{
    public class RequestController : BaseApiController
    {
        private readonly SensorRequestHelper _sensorRequestHelper = new SensorRequestHelper();
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly DataContext _context;

        public RequestController(DataContext context)
        {
            _context = context;
        }
        
        [Authorize]
        [HttpGet("sensor/temperature/{room}")]
        public async Task<ActionResult<WeatherDTO>> GetTemperature(string room)
        {
            var temperature = decimal.MinusOne;
            var humidity = decimal.MinusOne;
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), room);
            var roomIp = _context.GetRoomSensorIp(roomId);

            if (string.IsNullOrEmpty(roomIp))
            {
                return BadRequest("Room with such name doesn't exists!");
            }

            try
            {
                temperature =
                    decimal.Parse(_sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp),
                        SensorRequestType.Temperature)["temperature"].ToString());
                humidity =
                    decimal.Parse(_sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp),
                        SensorRequestType.Humidity)["humidity"].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while taking information about room with name {room}: {e.Message}");
            }

            return new WeatherDTO
            {
                Temperature = temperature,
                Humidity = humidity
            };
        }
        
        [Authorize]
        [HttpGet("sensor/additional/{room}")]
        public async Task<ActionResult<AdditionalInfoDTO>> GetAdditional(string room)
        {
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), room);
            var roomIp = _context.GetRoomSensorIp(roomId);
            
            if (string.IsNullOrEmpty(roomIp))
            {
                return BadRequest("Room with such name doesn't exists!");
            }
            
            var light = decimal.Parse(_sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp), SensorRequestType.Light)["light"].ToString());
            var soundDate = _sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp), SensorRequestType.Sound)["lastDetectionDate"].ToString();

            return new AdditionalInfoDTO
            {
                Light = light,
                SoundDate = soundDate
            };
        }

        [Authorize]
        [HttpGet("heater/on/{name}")]
        public async Task<ActionResult> TurnOnHeater(string name)
        {
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), name);

            if (roomId == -1)
            {
                return BadRequest("Room with such name doesn't exists or heater IP was not provided during room configuration!");
            }
            
            var room = _context.GetRoomBasedOnId(roomId);
            
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(room.HeaterIP)}/turnOn");
                
                if (response.IsSuccessStatusCode)
                {
                    room.HeaterEnabled = true;
                    _context.Rooms.Update(room);
                    await _context.SaveChangesAsync();
                    
                    return Ok("Heater turned on");
                }
            }
            catch(Exception)
            {
                return Problem("Failed to turn on heater. Not responding!");
            }

            return Problem("Failed to turn on heater!");
        }
        
        [Authorize]
        [HttpGet("heater/off/{name}")]
        public async Task<ActionResult> TurnOffHeater(string name)
        {
            var roomId = await _context.GetUserRoomIdBasedOnName(User.GetCurrentUser(), name);

            if (roomId == -1)
            {
                return BadRequest("Room with such name doesn't exists or heater IP was not provided during room configuration!");
            }
            
            var room = _context.GetRoomBasedOnId(roomId);

            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(room.HeaterIP)}/turnOff");
                
                if (response.IsSuccessStatusCode)
                {
                    room.HeaterEnabled = false;
                    _context.Rooms.Update(room);
                    await _context.SaveChangesAsync();
                    return Ok("Heater turned off");
                }
            }
            catch(Exception)
            {
                return Problem("Failed to turn off heater. Not responding!");
            }

            return Problem("Failed to turn off heater!");
        }
    }
}