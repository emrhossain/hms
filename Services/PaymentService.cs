using HMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ApplicationDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.Include(p => p.Reservation).ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.Include(p => p.Reservation)
                                          .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment?> AddPaymentAsync(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding payment");
                return null; // Returning null in case of failure
            }
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            try
            {
                var existingPayment = await _context.Payments.FindAsync(payment.PaymentId);
                if (existingPayment == null)
                    return false;

                _context.Entry(existingPayment).CurrentValues.SetValues(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating payment");
                return false; // Returning false in case of failure
            }
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                    return false;

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment");
                return false;
            }
        }

        public async Task<IEnumerable<Payment>?> SearchPaymentWithTerm(DateTime fromDate, DateTime toDate)
        {
            try
            {

                return await _context.Payments
                    .Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate)
                    .ToListAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error searching payments");
                return null;
            }
        }
    }
}
