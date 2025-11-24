using System.Diagnostics;
using LorcanaCardCollector.Models;
using Microsoft.AspNetCore.Mvc;

namespace LorcanaCardCollector.Controllers
{
    public class HomeController : Controller
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
