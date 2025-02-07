using System.ComponentModel.DataAnnotations;

namespace HMS.Data
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Customer selection is required.")]
        public int CustomerId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select a Room")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Check-in date required")]
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Check-out date required")]
        [ValidateDateRange]
        public DateTime CheckOutDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public DateTime LastModified_21180040 { get; set; } = DateTime.Now;
        public bool IsPaid { get; set; }
        public ApplicationUser User { get; set; }
        public Room Room { get; set; }
        public Customer Customer { get; set; }
    }
}
