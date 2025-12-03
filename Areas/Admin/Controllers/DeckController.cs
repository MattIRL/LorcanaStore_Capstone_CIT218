using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;
using Microsoft.AspNetCore.Authorization;

namespace LorcanaCardCollector.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeckController : Controller
    {
        private readonly CardsContext _context;

        public DeckController(CardsContext context)
        {
            _context = context;
        }

        // GET: Deck
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Decks.ToListAsync());
        }

        // GET: Deck/Details/5
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var deck = await _context.Decks
                .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                .FirstOrDefaultAsync(m => m.DeckId == id);

            if (deck == null)
                return NotFound();

            return View(deck);
        }


        // GET: Deck/Create
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deck/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create([Bind("DeckId,DeckName,DeckDescription,AccessKey")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }

        // GET: Deck/Edit/5
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var deck = await _context.Decks
                .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                .FirstOrDefaultAsync(d => d.DeckId == id);

            if (deck == null)
                return NotFound();

            var allCards = await _context.Cards.ToListAsync();

            var vm = new EditDeckViewModel
            {
                DeckId = deck.DeckId,
                DeckName = deck.DeckName,
                DeckDescription = deck.DeckDescription,
                Cards = allCards.Select(c => new CardCheckboxItem
                {
                    CardId = c.CardId,
                    CardName = c.CardName,
                    IsSelected = deck.DeckCards.Any(dc => dc.CardId == c.CardId)
                }).ToList()
            };

            return View(vm);
        }


        // POST: Deck/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(EditDeckViewModel vm)
        {
            var deck = await _context.Decks
                .Include(d => d.DeckCards)
                .FirstOrDefaultAsync(d => d.DeckId == vm.DeckId);

            if (deck == null) return NotFound();

            var existing = deck.DeckCards
                .Select(DbContext => DbContext.CardId)
                .ToList();

            var selected = vm.Cards
                .Where(c => c.IsSelected)
                .Select(c => c.CardId)
                .ToList();

            var toAdd = selected.Except(existing).ToList();
            var toRemove = existing.Except(selected).ToList();

            foreach (var cardId in toAdd)
            {
                deck.DeckCards.Add(new DeckCard
                {
                    DeckId = deck.DeckId,
                    CardId = cardId
                });
            }

            foreach (var cardId in toRemove)
            {
                var dc = deck.DeckCards.FirstOrDefault(x => x.CardId == cardId);
                if (dc != null)
                    _context.DeckCards.Remove(dc);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Deck/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _context.Decks
                .FirstOrDefaultAsync(m => m.DeckId == id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }

        // POST: Deck/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deck = await _context.Decks.FindAsync(id);
            if (deck != null)
            {
                _context.Decks.Remove(deck);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeckExists(int id)
        {
            return _context.Decks.Any(e => e.DeckId == id);
        }
    }
}
