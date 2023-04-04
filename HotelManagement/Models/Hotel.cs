using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [Url]
        public string Website { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
