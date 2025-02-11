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

        public async Task<bool> CheckOut(int reservationId)
        {
            try
            {
                var reservation = _dbContext.Reservations.Find(reservationId);
                if (reservation != null)
                {
                    reservation.CheckedOut = true;
                    _dbContext.Reservations.Update(reservation);
                    var room = _dbContext.Rooms.Find(reservation.RoomId);
                    if (room != null)
                    {
                        room.IsAvailable = true;
                        _dbContext.Rooms.Update(room);
                    }
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking out reservation");
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
                                    .Include(r => r.User)
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations");
                return default;
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateRange(DateTime fromDate, DateTime toDate, bool checkOutDate = false)
        {
            try
            {
                var date1 = fromDate.Date;
                var date2 = toDate.Date;

                var query = _dbContext.Reservations
                    .Include(r => r.Customer)
                    .Include(r => r.Room)
                    .AsQueryable();
                if (checkOutDate)
                {
                    query = query.Where(r => r.CheckOutDate.Date >= date1 && r.CheckOutDate.Date <= date2);
                }
                else
                {
                    query = query.Where(r => r.CheckInDate.Date >= date1 && r.CheckInDate.Date <= date2);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching reservation");
                return new List<Reservation>();
            }
        }

        public async Task<IEnumerable<Reservation>> SearchReservationAsync(string isPaid, string roomType)
        {
            try
            {
                return await _dbContext.Reservations
                    .Include(r => r.Room)
                    .Where(r =>
                        (r.IsPaid == (isPaid == "1") || isPaid == "") &&
                        EF.Functions.Like(r.Room.RoomType.ToString(), $"%{roomType}%"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching reservation");
                return new List<Reservation>();
            }
        }

        public async Task<bool> UpdatePaymentStatus(int reservationId, bool paid)
        {
            try
            {
                var reservation = _dbContext.Reservations.Find(reservationId);
                if (reservation != null)
                {
                    reservation.IsPaid = paid;
                    _dbContext.Reservations.Update(reservation);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status");
                return false;

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
