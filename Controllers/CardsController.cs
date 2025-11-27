using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LorcanaCardCollector.Models;

namespace LorcanaCardCollector.Controllers
{
    public class CardsController : Controller
    {
        private readonly CardsContext _context;

        public CardsController(CardsContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.ToListAsync());
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cards = await _context.Cards
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cards == null)
            {
                return NotFound();
            }

            return View(cards);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CardName,Franchise,Image_URL,Ink,GemColor,Willpower,Strength,SetName")] Cards card)
        {
            if (ModelState.IsValid)
            {
                // 1. Check if the card (identified by its unique ID) already exists
                var existingCard = await _context.Cards.FirstOrDefaultAsync(c => c.ID == card.ID);

                if (existingCard != null)
                {
                    // 2. If the card exists, update its details (Edit/Update operation)

                    existingCard.CardName = card.CardName;
                    existingCard.Franchise = card.Franchise;
                    existingCard.Image_URL = card.Image_URL;
                    existingCard.Ink = card.Ink;
                    existingCard.GemColor = card.GemColor;
                    existingCard.Willpower = card.Willpower;
                    existingCard.Strength = card.Strength;
                    existingCard.SetName = card.SetName;

                    // Mark the existing entity as modified
                    _context.Update(existingCard);
                }
                else
                {
                    // 3. If the card does not exist, add it as a new record (Insert operation)
                    _context.Add(card);
                }

                // 4. Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return the view
            return View(card);
        }

        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cards = await _context.Cards.FindAsync(id);
            if (cards == null)
            {
                return NotFound();
            }
            return View(cards);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,CardName,Franchise,Image_URL,Ink,GemColor,Willpower,Strength,SetName")] Cards cards)
        {
            if (id != cards.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cards);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardsExists(cards.ID))
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
            return View(cards);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cards = await _context.Cards
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cards == null)
            {
                return NotFound();
            }

            return View(cards);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cards = await _context.Cards.FindAsync(id);
            if (cards != null)
            {
                _context.Cards.Remove(cards);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Missing name parameter.");

            var url = $"https://api.lorcana-api.com/cards/fetch?search=name~{Uri.EscapeDataString(name)}";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return NotFound("No cards found.");

            var json = await response.Content.ReadAsStringAsync();

            return Content(json, "application/json");
        }


        private bool CardsExists(string id)
        {
            return _context.Cards.Any(e => e.ID == id);
        }
    }
}
