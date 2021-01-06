using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHomeAPI.Data;

namespace SmartHomeAPI.Helpers
{
    public static class RoomHelper
    {
        public static int[] GetUserRooms(this DataContext dataContext, string userName)
        {
            var result = dataContext.Rooms.Where(r => r.AppUser == userName);

            return result.Select(r => r.Id).ToArray();
        }
        
        public static int GetUserRoomBasedOnName(this DataContext dataContext, string userName, string roomName)
        {
            var result = dataContext.Rooms.FirstOrDefaultAsync(r => r.Name.ToLower().Contains(roomName.ToLower()) && r.AppUser == userName).Result;

            return result?.Id ?? -1;
        }

        public static string GetRoomSensorIp(this DataContext dataContext, int id)
        {
            return id == -1 ? string.Empty : dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result.SensorsIP;
        }
        
        public static string GetRoomHeaterIp(this DataContext dataContext, int id)
        {
            return id == - -1 ? string.Empty : dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result.HeaterIP;
        }
    }
}