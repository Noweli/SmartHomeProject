using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.DTOs;

namespace SmartHomeAPI.Controllers
{
    public class RequestController : BaseApiController
    {
        private readonly HttpClient _httpClient = new HttpClient();
        
        [Authorize]
        [HttpGet("temperature")]
        public async Task<ActionResult<WeatherDTO>> GetTemperature()
        {
            decimal temperature = 
                decimal.Parse(JObject.Parse(_httpClient.GetStringAsync("http://192.168.0.143/temperature").Result)["temperature"].ToString());
            decimal humidity = decimal.Parse(JObject.Parse(_httpClient.GetStringAsync("http://192.168.0.143/humidity").Result)["humidity"].ToString());
            return new WeatherDTO
            {
                Temperature = temperature,
                Humidity = humidity
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