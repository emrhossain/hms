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
            Logger.LogInformation("HotelService created");
        }

        ~HotelService()
        {
            Logger.LogInformation("HotelService disposed");
        }

        public async Task AddHotelAsync(Hotel hotel)
        {
            try
            {
                await _dbContext.Hotels.AddAsync(hotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving hotel");
            }
        }

        public async Task DeleteHotelAsync(int id)
        {
            try
            {
                var existingHotel = await _dbContext.Hotels.FindAsync(id);
                if (existingHotel != null)
                {
                    _dbContext.Hotels.Remove(existingHotel);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting hotel");
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
                return default;
            }
        }

        public async Task<IEnumerable<Hotel>?> SearchHotelsWithTerm(string searchTerm1, string searchTerm2)
        {
            try
            {
                return await _dbContext.Hotels
                                       .Where(h => h.Name.Contains(searchTerm1, StringComparison.OrdinalIgnoreCase) && h.Description.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase))
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching hotel");
                return default;
            }
        }

        public async Task UpdateHotelAsync(Hotel hotel)
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

                    _dbContext.Hotels.Update(existingHotel);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating hotel");
            }
        }
    }
}
