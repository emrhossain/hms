using HMS.Data;

namespace HMS.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> AddCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string cusomterName, string phoneNumber);
    }
}
