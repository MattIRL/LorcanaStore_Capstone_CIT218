using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;

namespace LorcanaCardCollector.Controllers
{
    public class DeckCardController : Controller
    {
        private readonly CardsContext _context;

        public DeckCardController(CardsContext context)
        {
            _context = context;
        }

        // GET: DeckCard
        public async Task<IActionResult> Index()
        {
            var cardsContext = _context.DeckCards
                .Include(d => d.Card)
                .Include(d => d.Deck);

            return View(await cardsContext.ToListAsync());
        }

        // GET: DeckCard/Details
        public async Task<IActionResult> Details(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .Include(d => d.Card)
                .Include(d => d.Deck)
                .FirstOrDefaultAsync(m => m.DeckId == deckId && m.CardId == cardId);

            if (deckCard == null)
                return NotFound();

            return View(deckCard);
        }

        // GET: DeckCard/Create
        public IActionResult Create()
        {
            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName");
            ViewData["DeckId"] = new SelectList(_context.Decks, "DeckId", "DeckName");
            return View();
        }

        // POST: DeckCard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,DeckId,QuantityInDeck")] DeckCard deckCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deckCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName", deckCard.CardId);
            ViewData["DeckId"] = new SelectList(_context.Decks, "DeckId", "DeckName", deckCard.DeckId);

            return View(deckCard);
        }

        // GET: DeckCard/Edit
        public async Task<IActionResult> Edit(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards.FindAsync(deckId, cardId);

            if (deckCard == null)
                return NotFound();

            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName", deckCard.CardId);
            ViewData["DeckId"] = new SelectList(_context.Decks, "DeckId", "DeckName", deckCard.DeckId);

            return View(deckCard);
        }

        // POST: DeckCard/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int deckId, string cardId, [Bind("CardId,DeckId,QuantityInDeck")] DeckCard deckCard)
        {
            if (deckId != deckCard.DeckId || cardId != deckCard.CardId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deckCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeckCardExists(deckId, cardId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CardId"] = new SelectList(_context.Cards, "CardId", "CardName", deckCard.CardId);
            ViewData["DeckId"] = new SelectList(_context.Decks, "DeckId", "DeckName", deckCard.DeckId);

            return View(deckCard);
        }

        // GET: DeckCard/Delete
        public async Task<IActionResult> Delete(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .Include(d => d.Card)
                .Include(d => d.Deck)
                .FirstOrDefaultAsync(m => m.DeckId == deckId && m.CardId == cardId);

            if (deckCard == null)
                return NotFound();

            return View(deckCard);
        }

        // POST: DeckCard/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards.FindAsync(deckId, cardId);

            if (deckCard != null)
                _context.DeckCards.Remove(deckCard);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeckCardExists(int deckId, string cardId)
        {
            return _context.DeckCards.Any(e => e.DeckId == deckId && e.CardId == cardId);
        }
    }
}
