﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHomeAPI.Data;
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
        
        public static int GetUserRoomIdBasedOnName(this DataContext dataContext, string userName, string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return -1;
            }
            
            var result = dataContext.Rooms
                .FirstOrDefaultAsync(r => r.Name.ToLower() == roomName.ToLower() && r.AppUser == userName);
            
            if (result == null)
            {
                result = dataContext.Rooms
                    .FirstOrDefaultAsync(r => r.Name.ToLower().Contains(roomName.ToLower()) && r.AppUser == userName);
            }

            return result?.Result.Id ?? -1;
        }
        
        public static AppRoom GetRoomBasedOnId(this DataContext dataContext, int id)
        {
            return dataContext.Rooms.FirstOrDefaultAsync(r => r.Id == id).Result;
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