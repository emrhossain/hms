﻿using HMS.Data;

namespace HMS.Services
{
    public interface IRoomService
    {
        Task<Room?> AddRoomAsync(Room room);
        Task<bool> DeleteRoomAsync(int id);
        Task<Room?> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<bool> UpdateRoomAsync(Room room);
        Task<IEnumerable<Room>> SearchRoomsAsync(string roomNumber, string roomType);
        Task<bool> UpdateStatus(int roomId, bool status);
    }
}
