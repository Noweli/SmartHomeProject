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
        private static readonly HttpClient client = new HttpClient();
        
        [Authorize]
        [HttpGet("temperature")]
        public async Task<ActionResult<WeatherDTO>> GetTemperature()
        {
            decimal temperature =
                decimal.Parse(JObject.Parse(client.GetStringAsync("http://192.168.0.144/temperature").Result)["temperature"].ToString());
            decimal humidity = decimal.Parse(JObject.Parse(client.GetStringAsync("http://192.168.0.144/humidity").Result)["humidity"].ToString());
            return new WeatherDTO
            {
                Temperature = temperature,
                Humidity = humidity
            };
        }
    }
}