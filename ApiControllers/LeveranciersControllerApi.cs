using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroupSpace23.ApiControllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LeveranciersControllerApi : ControllerBase
    {
        private readonly MyDbContext _context;

        public LeveranciersControllerApi(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/LeveranciersControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leverancier>>> GetLeverancier()
        {
          if (_context.Leverancier == null)
          {
              return NotFound();
          }
            return await _context.Leverancier.ToListAsync();
        }

        // GET: api/LeveranciersControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leverancier>> GetLeverancier(int id)
        {
          if (_context.Leverancier == null)
          {
              return NotFound();
          }
            var leverancier = await _context.Leverancier.FindAsync(id);

            if (leverancier == null)
            {
                return NotFound();
            }

            return leverancier;
        }

        // PUT: api/LeveranciersControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeverancier(int id, Leverancier leverancier)
        {
            if (id != leverancier.Id)
            {
                return BadRequest();
            }

            _context.Entry(leverancier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeverancierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LeveranciersControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leverancier>> PostLeverancier(Leverancier leverancier)
        {
          if (_context.Leverancier == null)
          {
              return Problem("Entity set 'MyDbContext.Leverancier'  is null.");
          }
            _context.Leverancier.Add(leverancier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeverancier", new { id = leverancier.Id }, leverancier);
        }

        // DELETE: api/LeveranciersControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeverancier(int id)
        {
            if (_context.Leverancier == null)
            {
                return NotFound();
            }
            var leverancier = await _context.Leverancier.FindAsync(id);
            if (leverancier == null)
            {
                return NotFound();
            }

            _context.Leverancier.Remove(leverancier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeverancierExists(int id)
        {
            return (_context.Leverancier?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
