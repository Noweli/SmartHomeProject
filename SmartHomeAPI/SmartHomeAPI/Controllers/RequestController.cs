using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.DTOs;
using SmartHomeAPI.Enums;
using SmartHomeAPI.Helpers;

namespace SmartHomeAPI.Controllers
{
    public class RequestController : BaseApiController
    {
        private readonly SensorRequestHelper _sensorRequestHelper = new SensorRequestHelper();
        private readonly HttpClient _httpClient = new HttpClient();
        
        [Authorize]
        [HttpGet("sensor/temperature")]
        public ActionResult<WeatherDTO> GetTemperature()
        {
            var temperature = decimal.Parse(_sensorRequestHelper.GetJsonData("http://192.168.0.143", SensorRequestType.Temperature)["temperature"].ToString());
            var humidity = decimal.Parse(_sensorRequestHelper.GetJsonData("http://192.168.0.143", SensorRequestType.Humidity)["humidity"].ToString());

            return new WeatherDTO
            {
                Temperature = temperature,
                Humidity = humidity
            };
        }
        
        [Authorize]
        [HttpGet("sensor/additional")]
        public ActionResult<AdditionalInfoDTO> GetHumidity()
        {
            var light = decimal.Parse(_sensorRequestHelper.GetJsonData("http://192.168.0.143", SensorRequestType.Light)["light"].ToString());
            var soundDate = _sensorRequestHelper.GetJsonData("http://192.168.0.143", SensorRequestType.Sound)["lastDetectionDate"].ToString();

            return new AdditionalInfoDTO
            {
                Light = light,
                SoundDate = soundDate
            };
        }

        [Authorize]
        [HttpGet("heater/on")]
        public async Task<ActionResult> TurnOnHeater()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync("http://192.168.0.94/turnOn");
                
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
        [HttpGet("heater/off")]
        public async Task<ActionResult> TurnOffHeater()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                var response = await _httpClient.GetAsync("http://192.168.0.94/turnOff");
                
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