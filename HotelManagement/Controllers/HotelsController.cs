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
    public class HotelsController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly Hotel _hotel;

        public HotelsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            var get_hotel = from h in _context.Hotels
                            orderby h.Id
                            select h;
            return get_hotel != null ?
                        View(await get_hotel.ToListAsync()) :
                        Problem("Entity set 'HotelDbContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City,Country,PhoneNumber,Website,Email")] Hotel hotel)
        {

            if (!ModelState.IsValid && ModelState.ErrorCount > 1)
            {
                return View();
            }
            else
            {
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5       
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,Country,PhoneNumber,Website,Email")] Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid && ModelState.ErrorCount > 1)
                {
                    return View(hotel);
                }
                else
                {
                    _context.Hotels.Update(hotel);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(hotel.Id))
                {
                    return NotFound();
                }
                else
                {
                    return View(hotel);
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'HotelDbContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Hotels/Rooms/5
        public async Task<IActionResult> Rooms(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.HotelRooms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            
            return View(hotel.HotelRooms.ToList());
        }


        private bool HotelExists(int id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}