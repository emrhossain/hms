using HMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RoomService> _logger;

        public RoomService(ILogger<RoomService> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Room?> AddRoomAsync(Room room)
        {
            try
            {
                room.LastModified_21180040 = DateTime.UtcNow;
                _dbContext.Rooms.Add(room);
                await _dbContext.SaveChangesAsync();
                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding room");
                return null;
            }
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            try
            {
                var existingRoom = await _dbContext.Rooms.FindAsync(id);
                if (existingRoom == null) return false;
                _dbContext.Rooms.Remove(existingRoom);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room");
                return false;
            }
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Rooms
                            .FirstOrDefaultAsync(r => r.RoomId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting room with id: {id}");
                return null;
            }
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            try
            {
                return await _dbContext.Rooms
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room list");
                return new List<Room>();
            }
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string roomNumber, string roomType)
        {
            try
            {
                return await _dbContext.Rooms
                    .Where(h =>
                        EF.Functions.Like(h.RoomNumber, $"%{roomNumber}%") &&
                        (h.RoomType.ToString() == roomType || roomType == string.Empty))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching rooms");
                return new List<Room>();
            }
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            try
            {
                var existingRoom = await _dbContext.Rooms.FindAsync(room.RoomId);
                if (existingRoom == null) return false;

                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomType = room.RoomType;
                existingRoom.PricePerNight = room.PricePerNight;
                existingRoom.IsAvailable = room.IsAvailable;
                existingRoom.LastModified_21180040 = DateTime.Now;

                _dbContext.Rooms.Update(existingRoom);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room");
                return false;
            }
        }

        public async Task<bool> UpdateStatus(int roomId, bool status)
        {
            try
            {
                var room = await _dbContext.Rooms.FindAsync(roomId);
                if (room == null)
                {
                    return false;
                }

                room.IsAvailable = status;
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating status");
                return false;
            }
        }
    }
}
