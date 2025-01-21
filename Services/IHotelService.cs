using HMS.Data;

namespace HMS.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>?> GetHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task AddHotelAsync(Hotel hotel);
        Task UpdateHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(int id);
        Task<IEnumerable<Hotel>?> SearchHotelsWithTerm(string searchTerm1, string searchTerm2);

    }
}
