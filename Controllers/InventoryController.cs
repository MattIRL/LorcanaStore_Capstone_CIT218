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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["StockSortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["Currentfilter"] = searchString;

                var cards = from c in _context.Inventories
                            .Include(c => c.Card)
                            select c;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(s => s.Card.CardName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    cards = cards.OrderByDescending(s => s.Card.CardName);
                    break;
                case "price":
                    cards = cards.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    cards = cards.OrderByDescending(s => s.Price);
                    break;
                case "quantity":
                    cards = cards.OrderBy(s => s.Quantity);
                    break;
                case "quantity_desc":
                    cards = cards.OrderByDescending(s => s.Quantity);
                    break;
                default:
                    cards = cards.OrderBy(s => s.Card.CardName);
                    break;
            }
            int pageSize = 4;
            return View(await PaginatedList<Inventory>.CreateAsync(cards.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        /*
        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }
        */
    }
}
