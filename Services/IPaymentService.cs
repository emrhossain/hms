using HMS.Data;

namespace HMS.Services
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment?> AddPaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int id);
        Task<IEnumerable<Payment>?> SearchPaymentWithTerm(DateTime fromDate, DateTime toDate);
    }
}
