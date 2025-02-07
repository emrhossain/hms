using System.ComponentModel.DataAnnotations;

namespace HMS.Data
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservation = (Reservation)validationContext.ObjectInstance;

            if (reservation.CheckInDate.Date > reservation.CheckOutDate.Date)
            {
                return new ValidationResult("Select valid date range.");
            }

            return ValidationResult.Success!;
        }
    }
}
