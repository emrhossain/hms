using System.ComponentModel.DataAnnotations;

namespace HMS.Data
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Name Required")]
        [MinLength(3, ErrorMessage = "Minimum Length 3")]
        public string Name { get; set; }
        public string Location { get; set; }
        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }
        [Phone(ErrorMessage = "Provide valid phone number")]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Provide valid email address")]
        public string Email { get; set; }
        public DateTime LastModified_21180040 { get; set; } = DateTime.Now;
        public ICollection<Room> Rooms { get; set; }
    }
}
