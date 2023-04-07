using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class HotelRoom
    {
        public int Id { get; set; }

        [Required]      
        public int HotelId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string RoomType { get; set; }
      
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity should be greater than 0")]
        public int Capacity { get; set; }
        
        [Required]
        public decimal PricePerNight { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        public Hotel Hotel { get; set; }
    }
}
