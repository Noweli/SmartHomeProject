using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.Enums;

namespace SmartHomeAPI.Helpers
{
    public class SensorRequestHelper
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public JObject GetJsonData(string url, SensorRequestType requestType)
        {
            var response = SendSensorRequest(url, requestType).Result;
            return string.IsNullOrEmpty(response) ? null : JObject.Parse(response);
        }

        private async Task<string> SendSensorRequest(string url, SensorRequestType requestType)
        {
            var result = requestType switch
            {
                SensorRequestType.Temperature => await _httpClient.GetStringAsync($"{url}/temperature"),
                SensorRequestType.Humidity => await _httpClient.GetStringAsync($"{url}/humidity"),
                SensorRequestType.Light => await _httpClient.GetStringAsync($"{url}/light"),
                SensorRequestType.Sound => await _httpClient.GetStringAsync($"{url}/sound"),
                _ => throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null)
            };

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }
    }
}