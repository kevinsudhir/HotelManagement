using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class HotelRoomsController : Controller
    {
        private readonly HotelDbContext _context;

        public HotelRoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: HotelRooms
        public async Task<IActionResult> Index()
        {
            var hotelDbContext = _context.HotelRooms.Include(h => h.Hotel);
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: HotelRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HotelRooms == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // GET: HotelRooms/Create
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            return View();
        }

        // POST: HotelRooms/Create        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,HotelId,RoomType,Capacity,PricePerNight,Description")] HotelRoom hotelRoom)
        {
            try
            {
                _context.Add(hotelRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", hotelRoom.HotelId);
                return View(hotelRoom);
            }
        }

        // GET: HotelRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HotelRooms == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.FindAsync(id);
            if (hotelRoom == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", hotelRoom.HotelId);
            return View(hotelRoom);
        }

        // POST: HotelRooms/Edit/5    
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HotelId,RoomType,Capacity,PricePerNight,Description")] HotelRoom hotelRoom)
        {
            if (id != hotelRoom.Id)
            {
                return NotFound();
            }

            try
            {
                _context.HotelRooms.Update(hotelRoom);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelRoomExists(hotelRoom.Id))
                {
                    return NotFound();
                }
                else
                {
                    ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", hotelRoom.HotelId);
                    return View(hotelRoom);
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: HotelRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HotelRooms == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // POST: HotelRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HotelRooms == null)
            {
                return Problem("Entity set 'HotelDbContext.HotelRooms'  is null.");
            }
            var hotelRoom = await _context.HotelRooms.FindAsync(id);
            if (hotelRoom != null)
            {
                _context.HotelRooms.Remove(hotelRoom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelRoomExists(int id)
        {
            return (_context.HotelRooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}