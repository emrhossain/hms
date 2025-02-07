using HMS.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HMS.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ApplicationDbContext context, ILogger<CustomerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> AddCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer");
                return null;
            }
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return false;
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return false;

                _context.Customers.Remove(customer);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                return false;
            }
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string cusomterName, string phoneNumber)
        {
            try
            {
                return await _context.Customers
                        .Where(c =>
                        EF.Functions.Like(c.Name, $"%{cusomterName}%") &&
                        EF.Functions.Like(c.PhoneNumber, $"%{phoneNumber}%"))
                        .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error searching customer");
                return new List<Customer>();
            }
        }
    }
}
