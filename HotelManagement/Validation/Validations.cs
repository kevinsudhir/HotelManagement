using HotelManagement.Data;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Validation
{
    public class ValidateModel
    {
        public static ValidationResult UniqueHotelName(string name, ValidationContext validationContext)
        {
            var dbContext = validationContext.GetService<HotelDbContext>();

            var currentHotel = validationContext.ObjectInstance as Hotel;

            var existingHotel = dbContext.Hotels.FirstOrDefault(h => h.Name == name);

            if (existingHotel != null && (currentHotel == null || existingHotel.Id != currentHotel.Id))
            {
                
                return new ValidationResult("The hotel name must be unique");
            }
            if (existingHotel != null)
            {
                dbContext.Entry(existingHotel).State = EntityState.Detached;
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
