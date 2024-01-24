using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;

namespace GroupSpace23.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectControllerApi : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProjectControllerApi(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetGroups()
        {
          if (_context.Projecten == null)
          {
              return NotFound();
          }
            return await _context.Projecten.ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetGroup(int id)
        {
          if (_context.Projecten == null)
          {
              return NotFound();
          }
            var @group = await _context.Projecten.FindAsync(id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Project @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostGroup(Project @group)
        {
          if (_context.Projecten == null)
          {
              return Problem("Entity set 'MyDbContext.Groups'  is null.");
          }
            _context.Projecten.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            if (_context.Projecten == null)
            {
                return NotFound();
            }
            var @group = await _context.Projecten.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Projecten.Remove(@group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return (_context.Projecten?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
