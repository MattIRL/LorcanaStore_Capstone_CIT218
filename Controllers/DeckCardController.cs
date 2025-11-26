using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;

namespace LorcanaCardCollector.Controllers
{
    public class DeckCardsController : Controller
    {
        private readonly CardsContext _context;

        public DeckCardsController(CardsContext context)
        {
            _context = context;
        }

        // GET: DeckCards
        public async Task<IActionResult> Index()
        {
            var deckCards = await _context.DeckCards
                .Include(dc => dc.Card)
                .Include(dc => dc.Deck)
                .ToListAsync();
            return View(deckCards);
        }

        // GET: DeckCards/Details?deckId=1&cardId=ARI-003
        public async Task<IActionResult> Details(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .Include(dc => dc.Card)
                .Include(dc => dc.Deck)
                .FirstOrDefaultAsync(dc => dc.DeckId == deckId && dc.CardId == cardId);

            if (deckCard == null)
                return NotFound();

            return View(deckCard);
        }

        // GET: DeckCards/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Decks = await _context.Decks.ToListAsync();
            ViewBag.Cards = await _context.Cards.ToListAsync();
            return View();
        }

        // POST: DeckCards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeckId,CardId,QuantityInDeck")] DeckCard deckCard)
        {
            if (ModelState.IsValid)
            {
                // Check if the combination already exists
                var existing = await _context.DeckCards
                    .FirstOrDefaultAsync(dc => dc.DeckId == deckCard.DeckId && dc.CardId == deckCard.CardId);
                if (existing != null)
                {
                    ModelState.AddModelError("", "This card is already in the deck.");
                    ViewBag.Decks = await _context.Decks.ToListAsync();
                    ViewBag.Cards = await _context.Cards.ToListAsync();
                    return View(deckCard);
                }

                _context.Add(deckCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Decks = await _context.Decks.ToListAsync();
            ViewBag.Cards = await _context.Cards.ToListAsync();
            return View(deckCard);
        }

        // GET: DeckCards/Edit?deckId=1&cardId=ARI-003
        public async Task<IActionResult> Edit(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .FirstOrDefaultAsync(dc => dc.DeckId == deckId && dc.CardId == cardId);

            if (deckCard == null)
                return NotFound();

            ViewBag.Decks = await _context.Decks.ToListAsync();
            ViewBag.Cards = await _context.Cards.ToListAsync();
            return View(deckCard);
        }

        // POST: DeckCards/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DeckId,CardId,QuantityInDeck")] DeckCard deckCard)
        {
            if (ModelState.IsValid)
            {
                _context.Update(deckCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Decks = await _context.Decks.ToListAsync();
            ViewBag.Cards = await _context.Cards.ToListAsync();
            return View(deckCard);
        }

        // GET: DeckCards/Delete?deckId=1&cardId=ARI-003
        public async Task<IActionResult> Delete(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .Include(dc => dc.Card)
                .Include(dc => dc.Deck)
                .FirstOrDefaultAsync(dc => dc.DeckId == deckId && dc.CardId == cardId);

            if (deckCard == null)
                return NotFound();

            return View(deckCard);
        }

        // POST: DeckCards/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int deckId, string cardId)
        {
            var deckCard = await _context.DeckCards
                .FirstOrDefaultAsync(dc => dc.DeckId == deckId && dc.CardId == cardId);

            if (deckCard != null)
            {
                _context.DeckCards.Remove(deckCard);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
