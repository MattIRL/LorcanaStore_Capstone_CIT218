using LorcanaCardCollector.Models;
using LorcanaCardCollector.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LorcanaCardCollector.Controllers
{

    public class CardsController : Controller
    {
        private readonly CardsContext _context;
        private readonly LorcanaApiService _apiService;

        public CardsController(CardsContext context, LorcanaApiService apiService)
        {
            _context = context;
            _apiService = apiService;
            
        }

        // Search Functionality
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Missing name parameter.");

            var cards = await _apiService.SearchCardsAsync(name);
            return Json(cards);
        }


        // GET: Cards
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            int pageSize = 4;

            var cards = from c in _context.Cards
                        .OrderBy(c => c.CardName)
                        select c;

            return View(await PaginatedList<Cards>
                .CreateAsync(cards.AsNoTracking(), pageNumber ?? 1, pageSize));
            /*
            return View(await _context.Cards
                    .OrderBy(c => c.CardName)
                    .ToListAsync());
            */
        }

        // GET: Cards/Details/5
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cards = await _context.Cards
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (cards == null)
            {
                return NotFound();
            }

            return View(cards);
        }

        // GET: Cards/Create
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create([Bind("CardId,CardName,Franchise,Image_URL,Ink,GemColor,Willpower,Strength,SetName")] Cards card)
        {
            if (ModelState.IsValid)
            {
                // 1. Check if the card (identified by its unique ID) already exists
                var existingCard = await _context.Cards.FirstOrDefaultAsync(c => c.CardId == card.CardId);

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

        private bool CardsExists(string id) => _context.Cards.Any(e => e.CardId == id);
    }
}
