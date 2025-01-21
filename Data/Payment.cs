namespace HMS.Data
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime LastModified_21180040 { get; set; }
        public Reservation Reservation { get; set; }
    }
}
