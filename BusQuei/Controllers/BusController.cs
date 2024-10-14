using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusQuei.Context;
using BusQuei.Models;

namespace BusQuei.Controllers
{
    public class BusController : Controller
    {
        private readonly AppDbContext _context;

        public BusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bus
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Buses.Include(b => b.Route);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Bus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses
                .Include(b => b.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // GET: Bus/Create
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Arrival");
            return View();
        }

        // POST: Bus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LicensePlate,Model,Capacity,Status,RouteId")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Arrival", bus.RouteId);
            return View(bus);
        }

        // GET: Bus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Arrival", bus.RouteId);
            return View(bus);
        }

        // POST: Bus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LicensePlate,Model,Capacity,Status,RouteId")] Bus bus)
        {
            if (id != bus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(bus.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Arrival", bus.RouteId);
            return View(bus);
        }

        // GET: Bus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses
                .Include(b => b.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // POST: Bus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus != null)
            {
                _context.Buses.Remove(bus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.Id == id);
        }
    }
}
