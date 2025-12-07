using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;
using Microsoft.AspNetCore.Authorization;

namespace LorcanaCardCollector.Controllers
{
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

        // GET: Deck/Edit/5 (QUANTITY VERSION)
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

                Cards = allCards.Select(c =>
                {
                    var deckCard = deck.DeckCards.FirstOrDefault(dc => dc.CardId == c.CardId);

                    return new DeckCardItem
                    {
                        CardId = c.CardId,
                        CardName = c.CardName,
                        ImageUrl = c.Image_URL,          // optional
                        Quantity = deckCard?.QuantityInDeck ?? 0
                    };
                }).ToList()
            };

            return View(vm);
        }


        // POST: Deck/Edit/5 (QUANTITY VERSION)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(EditDeckViewModel vm)
        {
            var deck = await _context.Decks
                .Include(d => d.DeckCards)
                .FirstOrDefaultAsync(d => d.DeckId == vm.DeckId);

            if (deck == null)
                return NotFound();

            // Cards user submitted with Quantity > 0
            var submitted = vm.Cards.Where(c => c.Quantity > 0).ToList();

            // Current DB cards
            var existing = deck.DeckCards.ToList();

            //
            // 1. ADD or UPDATE cards
            //
            foreach (var cardVm in submitted)
            {
                var existingCard = existing.FirstOrDefault(x => x.CardId == cardVm.CardId);

                if (existingCard == null)
                {
                    // Add
                    deck.DeckCards.Add(new DeckCard
                    {
                        DeckId = deck.DeckId,
                        CardId = cardVm.CardId,
                        QuantityInDeck = cardVm.Quantity
                    });
                }
                else
                {
                    // Update quantity
                    if (existingCard.QuantityInDeck != cardVm.Quantity)
                    {
                        existingCard.QuantityInDeck = cardVm.Quantity;
                    }
                }
            }

            //
            // 2. REMOVE cards now set to Quantity = 0
            //
            foreach (var existingCard in existing)
            {
                var submittedCard = submitted.FirstOrDefault(c => c.CardId == existingCard.CardId);

                if (submittedCard == null)
                {
                    _context.DeckCards.Remove(existingCard);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Deck/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var deck = await _context.Decks.FirstOrDefaultAsync(m => m.DeckId == id);
            if (deck == null)
                return NotFound();

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
                _context.Decks.Remove(deck);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeckExists(int id)
        {
            return _context.Decks.Any(e => e.DeckId == id);
        }
    }
}
