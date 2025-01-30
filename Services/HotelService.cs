using HMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HotelService> Logger;

        public HotelService(ApplicationDbContext dbContext, ILogger<HotelService> logger)
        {
            _dbContext = dbContext;
            Logger = logger;
        }

        public async Task<Hotel?> AddHotelAsync(Hotel hotel)
        {
            try
            {
                var savedHotel = await _dbContext.Hotels.AddAsync(hotel);
                await _dbContext.SaveChangesAsync();
                return savedHotel.Entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving hotel");
                return null;
            }
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            try
            {
                var existingHotel = await _dbContext.Hotels.FindAsync(id);
                if (existingHotel != null)
                {
                    _dbContext.Hotels.Remove(existingHotel);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting hotel");
                return false;
            }
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Hotels.FindAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error getting hotel with id: {id}");
                return default;
            }
        }

        public async Task<IEnumerable<Hotel>?> GetHotelsAsync()
        {
            try
            {
                return await _dbContext.Hotels.ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting hotels");
                return new List<Hotel>();
            }
        }

        public async Task<IEnumerable<Hotel>?> SearchHotelsWithTerm(string searchTerm1, string searchTerm2)
        {
            try
            {
                return await _dbContext.Hotels
                    .Where(h =>
                        EF.Functions.Like(h.Name, $"%{searchTerm1}%") &&
                        EF.Functions.Like(h.Description, $"%{searchTerm2}%"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching hotels with terms: {SearchTerm1}, {SearchTerm2}", searchTerm1, searchTerm2);
                return Enumerable.Empty<Hotel>();
            }
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel)
        {
            try
            {
                var existingHotel = await _dbContext.Hotels.FindAsync(hotel.HotelId);
                if (existingHotel != null)
                {
                    existingHotel.Name = hotel.Name;
                    existingHotel.Description = hotel.Description;
                    existingHotel.Location = hotel.Location;
                    existingHotel.Email = hotel.Email;
                    existingHotel.PhoneNumber = hotel.PhoneNumber;
                    existingHotel.LastModified_21180040 = DateTime.Now;

                    _dbContext.Hotels.Update(existingHotel);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating hotel");
                return false;
            }
        }
    }
}
