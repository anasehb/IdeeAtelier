using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GroupSpace23.Areas.Identity.Data;

namespace GroupSpace23.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserManager<GroupSpace23User> _userManager;

        public GroupsController(MyDbContext context, UserManager<GroupSpace23User> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        // GET: Groups
        public async Task<IActionResult> Index(string Name)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            List<Project> groups = new List<Project>();

            if (User.IsInRole("SystemAdministrator"))
            {
                // Als de gebruiker een systeembeheerder is, alle projecten weergeven
                groups = _context.Projecten
                    .Where(u => u.Name != "Dummy" && (u.Name.Contains(Name) || string.IsNullOrEmpty(Name)))
                    .ToList();
            }
            else
            {
                // Als de gebruiker geen systeembeheerder is, alleen zijn eigen projecten weergeven
                groups = _context.Projecten
                    .Where(u => u.Name != "Dummy" && u.StartedById == currentUser.Id && (u.Name.Contains(Name) || string.IsNullOrEmpty(Name)))
                    .ToList();
            }

            ViewData["Name"] = Name;

            return View(groups);
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projecten == null)
            {
                return NotFound();
            }

            var @group = await _context.Projecten
                .Include(m => m.StartedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View(new Project());
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Started,Ended,StartedById")] Project @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                group.StartedById = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projecten == null)
            {
                return NotFound();
            }

            var @group = await _context.Projecten.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Started,Ended,StartedById")] Project @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(@group.Id))
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
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projecten == null)
            {
                return NotFound();
            }

            var Project = await _context.Projecten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Project == null)
            {
                return NotFound();
            }

            return View(Project);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InventoryItem == null)
            {
                return Problem("Entity set 'MyDbContext.Project'  is null.");
            }
            var Project = await _context.Projecten.FindAsync(id);
            if (Project != null)
            {
                _context.Projecten.Remove(Project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (_context.InventoryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}