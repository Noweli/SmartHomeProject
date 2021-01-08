using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHomeAPI.Data;
using SmartHomeAPI.DTOs;
using SmartHomeAPI.Entity;

namespace SmartHomeAPI.Helpers
{
    public static class RoomHelper
    {
        public static AppRoom[] GetUserRooms(this DataContext dataContext, string userName)
        {
            var result = dataContext.Rooms.Where(r => r.AppUser == userName);

            return result.ToArray();
        }
        
        public static int[] GetUserRoomsId(this DataContext dataContext, string userName)
        {
            var result = dataContext.Rooms.Where(r => r.AppUser == userName);

            return result.Select(r => r.Id).ToArray();
        }
        
        public static int[] GetUserRoomsIdWithAutoHeatEnabled(this DataContext dataContext, string userName)
        {
            var result = dataContext.Rooms.Where(r => r.AppUser == userName && r.AutoHeatEnabled);

            return result.Select(r => r.Id).ToArray();
        }
        
        public static async Task<int> GetUserRoomIdBasedOnName(this DataContext dataContext, string userName, string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return -1;
            }
            
            var result = await dataContext.Rooms
                .FirstOrDefaultAsync(r => r.Name.ToLower() == roomName.ToLower() && r.AppUser == userName);
            
            if (result == null)
            {
                result = await dataContext.Rooms
                    .FirstOrDefaultAsync(r => r.Name.ToLower().Contains(roomName.ToLower()) && r.AppUser == userName);
            }

            return result?.Id ?? -1;
        }
        
        public static AppRoom GetRoomBasedOnId(this DataContext dataContext, int id)
        {
            return dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result;
        }

        public static string GetRoomSensorIp(this DataContext dataContext, int id)
        {
            return id == -1 ? string.Empty : dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result?.SensorsIP ?? string.Empty;
        }
        
        public static string GetRoomHeaterIp(this DataContext dataContext, int id)
        {
            return id == - -1 ? string.Empty : dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result.HeaterIP;
        }

        public static string GetHeaterParmeters(this DataContext dataContext, AppRoom room)
        {
            StringBuilder result = new StringBuilder("?");
            string heaterIp = room.HeaterIP;
            
            if (!heaterIp.Contains("http"))
            {
                heaterIp = $"http://{heaterIp}/";
            }

            result.Append("minTemp=").Append(room.MinTemp).Append("&maxTemp=").Append(room.MaxTemp)
                .Append("&heaterAddress=").Append(heaterIp);

            return result.ToString();
        }

        public static RoomDTO ConverAppRoomToRoomDTO(this AppRoom room)
        {
            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                AppUser = room.AppUser,
                MaxTemp = room.MaxTemp,
                MinTemp = room.MinTemp,
                AutoHeatEnabled = room.AutoHeatEnabled,
                HeaterIp = room.HeaterIP,
                SensorsIp = room.SensorsIP,
                HeaterEnabled = room.HeaterEnabled
            };
        }
    }
}