using System.ComponentModel.DataAnnotations;

namespace HMS.Data
{
    public class Room
    {
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Hotel Required")]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Room Number Required")]
        public string RoomNumber { get; set; }
        [Required(ErrorMessage = "Room Type Required")]
        public string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastModified_21180040 { get; set; } = DateTime.Now;
        public Hotel Hotel { get; set; }
    }
}
