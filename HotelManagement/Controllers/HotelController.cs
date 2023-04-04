using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _service;

        public HotelController(HotelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Hotel>> GetHotels()
        {
            return await _service.GetHotelsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _service.GetHotelAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        [HttpGet("{hotelId}/rooms")]
        public async Task<IEnumerable<HotelRoom>> GetHotelRooms(int hotelId)
        {
            return await _service.GetHotelRoomsAsync(hotelId);
        }

        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<HotelRoom>> GetHotelRoom(int id)
        {
            var hotelRoom = await _service.GetHotelRoomAsync(id);

            if (hotelRoom == null)
            {
                return NotFound();
            }

            return hotelRoom;
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> AddHotel([FromBody] Hotel hotel)
        {

            var addedHotel = await _service.AddHotelAsync(hotel);

            return CreatedAtAction(nameof(GetHotel), new { id = addedHotel.Id }, addedHotel);
        }

        [HttpPost("{hotelId}/rooms")]
        public async Task<ActionResult<HotelRoom>> AddHotelRoom(int hotelId, [FromBody] HotelRoom hotelRoom)
        {
            var hotel = await _service.GetHotelAsync(hotelId);

            if (hotel == null)
            {
                return NotFound();
            }

            hotelRoom.HotelId = hotelId;


            var addedHotelRoom = await _service.AddHotelRoomAsync(hotelRoom);

            return CreatedAtAction(nameof(GetHotelRoom), new { id = addedHotelRoom.Id }, addedHotelRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] Hotel hotel)
        {
            var updatedHotel = await _service.UpdateHotelAsync(id, hotel);

            if (updatedHotel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> UpdateHotelRoom(int id, [FromBody] HotelRoom hotelRoom)
        {
            var updatedHotelRoom = await _service.UpdateHotelRoomAsync(id, hotelRoom);

            if (updatedHotelRoom == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var deleted = await _service.DeleteHotelAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("rooms/{id}")]
        public async Task<IActionResult> DeleteHotelRoom(int id)
        {
            var deleted = await _service.DeleteHotelRoomAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
