namespace HMS.Data
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime LastModified_21180040 { get; set; }
        public ApplicationUser User { get; set; }
        public Room Room { get; set; }
    }
}
