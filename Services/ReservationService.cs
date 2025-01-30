using HMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace HMS.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ApplicationDbContext dbContext, ILogger<ReservationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
        {
            try
            {
                await _dbContext.Reservations.AddAsync(reservation);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reservation");
                return false;
            }
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            try
            {
                var existingReservation = await _dbContext.Reservations.FindAsync(id);
                if (existingReservation != null)
                {
                    _dbContext.Reservations.Remove(existingReservation);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reservation");
                return false;
            }
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Reservations
                                    .Include(r => r.Room)
                                    .Include(r => r.User)
                                    .FirstOrDefaultAsync(r => r.ReservationId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reservation with id: {id}");
                return default;
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            try
            {
                return await _dbContext.Reservations
                                    .Include(r => r.Room)
                                        .ThenInclude(room => room.Hotel) // Include Room's Hotel
                                    .Include(r => r.User)
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations");
                return default;
            }
        }

        public async Task<IEnumerable<Reservation>> SearchReservationAsync(string hotelName, string roomType)
        {
            try
            {
                return await _dbContext.Reservations
                    .Include(r => r.Room)
                            .ThenInclude(room => room.Hotel) // Include Room's Hotel
                    .Where(h =>
                        EF.Functions.Like(h.Room.Hotel.Name, $"%{hotelName}%") &&
                        EF.Functions.Like(h.Room.RoomType, $"%{roomType}%"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching reservation");
                return new List<Reservation>();
            }
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            try
            {
                _dbContext.Reservations.Update(reservation);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reservation");
                return false;
            }
        }

    }
}
