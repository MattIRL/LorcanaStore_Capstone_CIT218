using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;
using Microsoft.AspNetCore.Authorization;

namespace LorcanaCardCollector.Controllers
{
    public class InventoryController : Controller
    {
        private readonly CardsContext _context;

        public InventoryController(CardsContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var cardsContext = _context.Inventories.Include(i => i.Card);
            return View(await cardsContext
                .OrderBy(c => c.Card.CardName)
                .ToListAsync());
        }

        // GET: Inventory/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Card)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CardId,Quantity,Cost,Price")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                inventory.CardId = inventory.CardId.ToUpperInvariant();
                try
                {
                    _context.Add(inventory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    // THIS WILL EXPOSE THE DB ERROR:
                    var innerException = ex.InnerException?.Message;
                    ModelState.AddModelError(string.Empty, $"Database Save Failed! Error: {innerException}");
                }
            }

            // ... rest of the code to re-populate ViewBag and return View(inventory)
            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName", inventory.CardId);
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Card)
                .FirstOrDefaultAsync(i =>i.InventoryId == id);

            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName", inventory.CardId);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("InventoryId,CardId,Quantity,Cost,Price")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
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
            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardId", inventory.CardId);
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Card)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }
    }
}
