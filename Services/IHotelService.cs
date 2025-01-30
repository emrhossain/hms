using HMS.Data;

namespace HMS.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>?> GetHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel?> AddHotelAsync(Hotel hotel);
        Task<bool> UpdateHotelAsync(Hotel hotel);
        Task<bool> DeleteHotelAsync(int id);
        Task<IEnumerable<Hotel>?> SearchHotelsWithTerm(string searchTerm1, string searchTerm2);

    }
}
