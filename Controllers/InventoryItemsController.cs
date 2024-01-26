using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Identity;
using GroupSpace23.Areas.Identity.Data;

namespace GroupSpace23.Controllers
{
    public class InventoryItemsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserManager<GroupSpace23User> _userManager;

        public InventoryItemsController(MyDbContext context, UserManager<GroupSpace23User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: InventoryItems
        public async Task<IActionResult> Index(string Name)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            if (User.IsInRole("SystemAdministrator"))
            {
                // Als de gebruiker een systeembeheerder is, alle inventarisitems weergeven
                inventoryItems = await _context.InventoryItem
                    .Where(item => item.Name != "Dummy" && (item.Name.Contains(Name) || string.IsNullOrEmpty(Name)))
                    .ToListAsync();
            }
            else
            {
                // Als de gebruiker geen systeembeheerder is, alleen zijn eigen inventarisitems weergeven
                inventoryItems = await _context.InventoryItem
                    .Where(item => item.Name != "Dummy" && item.OwnerId == currentUser.Id && (item.Name.Contains(Name) || string.IsNullOrEmpty(Name)))
                    .ToListAsync();
            }

            ViewData["Name"] = Name;

            return View(inventoryItems);
        }
        // GET: InventoryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InventoryItem == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // GET: InventoryItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity,Price")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InventoryItem == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItem.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Quantity,Price")] InventoryItem inventoryItem)
        {
            if (id != inventoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryItemExists(inventoryItem.Id))
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
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InventoryItem == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InventoryItem == null)
            {
                return Problem("Entity set 'MyDbContext.InventoryItem'  is null.");
            }
            var inventoryItem = await _context.InventoryItem.FindAsync(id);
            if (inventoryItem != null)
            {
                _context.InventoryItem.Remove(inventoryItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryItemExists(int id)
        {
          return (_context.InventoryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
