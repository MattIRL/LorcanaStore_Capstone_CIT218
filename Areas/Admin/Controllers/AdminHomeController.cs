using LorcanaCardCollector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LorcanaCardCollector.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        // Search button logic for displaying error or calling card images
        // Will need foreach on the results from https://api.lorcana-api.com/cards/fetch?search=name~micke
        public async Task<IActionResult> Index(LorcanaCardCollectorModel model)
        {
                var cards = await model.FetchCardAsync();
                ViewBag.Cards = cards;
                return View(model);
        }

    }
}
