using HotelManagement.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(Validations), "UniqueHotelName", ErrorMessage = "The {0} must be unique")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [CustomValidation(typeof(Validations), "IsValidPhoneNumber", ErrorMessage = "Phone numbers must be numeric but could start with “+”")]
        public string PhoneNumber { get; set; }

        [Required]
        [Url]
        public string Website { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        public ICollection<HotelRoom> HotelRooms { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validate using data annotations
            Validator.TryValidateObject(this, new ValidationContext(this, serviceProvider: null, items: null), results, true);

            // Validate using custom validation methods
            if (!string.IsNullOrEmpty(Name))
            {
                var uniqueNameResult = Validations.UniqueHotelName(Name, validationContext);
                if (uniqueNameResult != ValidationResult.Success)
                {
                    results.Add(uniqueNameResult);
                }
            }

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                var phoneResult = Validations.IsValidPhoneNumber(PhoneNumber, validationContext);
                if (phoneResult != ValidationResult.Success)
                {
                    results.Add(phoneResult);
                }
            }            

            foreach (var result in results)
            {
                yield return result;
            }
        }
    }


    public class HotelRoom
    {
        public int Id { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        [MaxLength(100)]
        public string RoomType { get; set; }

        [Required(ErrorMessage = "Please enter the number of guests.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of guests.")]
        public int Capacity { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public Hotel Hotel { get; set; }
    }
}
