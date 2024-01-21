﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;

namespace GroupSpace23.Controllers
{
    public class LeveranciersController : Controller
    {
        private readonly MyDbContext _context;

        public LeveranciersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Leveranciers
        public async Task<IActionResult> Index()
        {
              return _context.Leverancier != null ? 
                          View(await _context.Leverancier.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.Leverancier'  is null.");
        }

        // GET: Leveranciers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leverancier == null)
            {
                return NotFound();
            }

            var leverancier = await _context.Leverancier
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leverancier == null)
            {
                return NotFound();
            }

            return View(leverancier);
        }

        // GET: Leveranciers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leveranciers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Leverancier leverancier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leverancier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leverancier);
        }

        // GET: Leveranciers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leverancier == null)
            {
                return NotFound();
            }

            var leverancier = await _context.Leverancier.FindAsync(id);
            if (leverancier == null)
            {
                return NotFound();
            }
            return View(leverancier);
        }

        // POST: Leveranciers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Leverancier leverancier)
        {
            if (id != leverancier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leverancier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeverancierExists(leverancier.Id))
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
            return View(leverancier);
        }

        // GET: Leveranciers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leverancier == null)
            {
                return NotFound();
            }

            var leverancier = await _context.Leverancier
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leverancier == null)
            {
                return NotFound();
            }

            return View(leverancier);
        }

        // POST: Leveranciers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leverancier == null)
            {
                return Problem("Entity set 'MyDbContext.Leverancier'  is null.");
            }
            var leverancier = await _context.Leverancier.FindAsync(id);
            if (leverancier != null)
            {
                _context.Leverancier.Remove(leverancier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeverancierExists(int id)
        {
          return (_context.Leverancier?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
