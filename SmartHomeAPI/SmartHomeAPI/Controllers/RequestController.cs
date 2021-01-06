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
        public ActionResult<WeatherDTO> GetTemperature(string room)
        {
            var roomIp = _context.GetRoomSensorIp(_context.GetUserRoomBasedOnName(User.GetCurrentUser(), room));

            if (string.IsNullOrEmpty(roomIp))
            {
                return BadRequest("Room with such name doesn't exists!");
            }
            
            var temperature = decimal.Parse(_sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp), SensorRequestType.Temperature)["temperature"].ToString());
            var humidity = decimal.Parse(_sensorRequestHelper.GetJsonData(_sensorRequestHelper.GetHttpUrl(roomIp), SensorRequestType.Humidity)["humidity"].ToString());

            return new WeatherDTO
            {
                Temperature = temperature,
                Humidity = humidity
            };
        }
        
        [Authorize]
        [HttpGet("sensor/additional/{room}")]
        public ActionResult<AdditionalInfoDTO> GetAdditional(string room)
        {
            var roomIp = _context.GetRoomSensorIp(_context.GetUserRoomBasedOnName(User.GetCurrentUser(), room));
            
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
        [HttpGet("heater/on/{room}")]
        public async Task<ActionResult> TurnOnHeater(string room)
        {
            var roomIp = _context.GetRoomHeaterIp(_context.GetUserRoomBasedOnName(User.GetCurrentUser(), room));
            
            if (string.IsNullOrEmpty(roomIp))
            {
                return BadRequest("Room with such name doesn't exists or heater IP was not provided during room configuration!");
            }
            
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(roomIp)}/turnOn");
                
                if (response.IsSuccessStatusCode)
                {
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
        [HttpGet("heater/off/{room}")]
        public async Task<ActionResult> TurnOffHeater(string room)
        {
            var roomIp = _context.GetRoomHeaterIp(_context.GetUserRoomBasedOnName(User.GetCurrentUser(), room));
            
            if (string.IsNullOrEmpty(roomIp))
            {
                return BadRequest("Room with such name doesn't exists or heater IP was not provided during room configuration!");
            }
            
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync($"{_sensorRequestHelper.GetHttpUrl(roomIp)}/turnOff");
                
                if (response.IsSuccessStatusCode)
                {
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