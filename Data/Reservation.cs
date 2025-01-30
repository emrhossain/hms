using System.ComponentModel.DataAnnotations;

namespace HMS.Data
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select a Room")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Check-in date required")]
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public DateTime LastModified_21180040 { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }
        public Room Room { get; set; }
    }
}
