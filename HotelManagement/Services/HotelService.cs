using HotelManagement.Data;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Services
{
    public class HotelService
    {
        private readonly HotelDbContext _context;

        public HotelService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await _context.Hotels.Include(h => h.HotelRooms).ToListAsync();
        }

        public async Task<Hotel> GetHotelAsync(int id)
        {
            return await _context.Hotels.Include(h => h.HotelRooms).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<HotelRoom>> GetHotelRoomsAsync(int hotelId)
        {
            return await _context.HotelRooms.Where(hr => hr.HotelId == hotelId).ToListAsync();
        }

        public async Task<HotelRoom> GetHotelRoomAsync(int hotelRoomId)
        {
            return await _context.HotelRooms.FirstOrDefaultAsync(hr => hr.Id == hotelRoomId);
        }

        public async Task<Hotel> AddHotelAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<HotelRoom> AddHotelRoomAsync(HotelRoom hotelRoom)
        {
            _context.HotelRooms.Add(hotelRoom);
            await _context.SaveChangesAsync();
            return hotelRoom;
        }

        public async Task<Hotel> UpdateHotelAsync(int id, Hotel hotel)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);

            if (existingHotel == null)
            {
                return null;
            }

            existingHotel.Name = hotel.Name;
            existingHotel.Address = hotel.Address;
            existingHotel.City = hotel.City;
            existingHotel.Country = hotel.Country;
            existingHotel.PhoneNumber = hotel.PhoneNumber;
            existingHotel.Website = hotel.Website;
            existingHotel.Email = hotel.Email;

            await _context.SaveChangesAsync();

            return existingHotel;
        }

        public async Task<HotelRoom> UpdateHotelRoomAsync(int id, HotelRoom hotelRoom)
        {
            var existingHotelRoom = await _context.HotelRooms.FindAsync(id);

            if (existingHotelRoom == null)
            {
                return null;
            }

            existingHotelRoom.RoomType = hotelRoom.RoomType;
            existingHotelRoom.Capacity = hotelRoom.Capacity;
            existingHotelRoom.PricePerNight = hotelRoom.PricePerNight;
            existingHotelRoom.Description = hotelRoom.Description;

            await _context.SaveChangesAsync();

            return existingHotelRoom;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);

            if (existingHotel == null)
            {
                return false;
            }

            _context.Hotels.Remove(existingHotel);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteHotelRoomAsync(int id)
        {
            var existingHotelRoom = await _context.HotelRooms.FindAsync(id);

            if (existingHotelRoom == null)
            {
                return false;
            }

            _context.HotelRooms.Remove(existingHotelRoom);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
