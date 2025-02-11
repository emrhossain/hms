using HMS.Data;

namespace HMS.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task<bool> AddReservationAsync(Reservation reservation);
        Task<bool> UpdateReservationAsync(Reservation reservation);
        Task<bool> DeleteReservationAsync(int id);
        Task<IEnumerable<Reservation>> SearchReservationAsync(string isPaid, string roomType);
        Task<IEnumerable<Reservation>> GetReservationsByDateRange(DateTime fromDate, DateTime toDate, bool checkOutDate = false);
        Task<bool> UpdatePaymentStatus(int reservationId, bool paid);
        Task<bool> CheckOut(int reservationId);

    }
}
