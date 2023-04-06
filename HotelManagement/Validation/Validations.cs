using HotelManagement.Data;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Validation
{
    public static class Validations
    {
        public static ValidationResult AtLeastOnePersonPerRoom(int? capacity, ValidationContext context)
        {
            if (capacity.HasValue && capacity.Value < 1)
            {
                return new ValidationResult("A booking must have at least one adult per room.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult RoomMustHaveHotel(int? hotelId, ValidationContext context)
        {
            if (hotelId.HasValue && context.GetService(typeof(HotelDbContext)) is HotelDbContext dbContext)
            {
                var hotel = dbContext.Hotels.Find(hotelId.Value);
                if (hotel == null)
                {
                    return new ValidationResult("A room can’t exist without a hotel.");
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult UniqueHotelName(string name, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(name) && context.GetService(typeof(HotelDbContext)) is HotelDbContext dbContext)
            {
                var hotel = dbContext.Hotels.FirstOrDefault(h => h.Name == name);
                if (hotel != null)
                {
                    return new ValidationResult("You cannot enter two identical hotel names.");
                }
            }

            return ValidationResult.Success;
        }
     

        public static ValidationResult IsValidPhoneNumber(string phoneNumber, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.Substring(1);
                }

                if (!long.TryParse(phoneNumber, out _))
                {
                    return new ValidationResult("Phone numbers must be numeric but could start with “+”");
                }
            }

            return ValidationResult.Success;
        }
    }
}
