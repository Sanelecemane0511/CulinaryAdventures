using CulinaryAdventures.Data;
using CulinaryAdventures.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulinaryAdventures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string? category, string sort)
        {
            var q = _context.Recipes.AsQueryable();
            if (!string.IsNullOrEmpty(category))
                q = q.Where(r => r.Category == category);

            q = sort switch
            {
                "time" => q.OrderBy(r => r.PrepTimeMin),
                "rating" => q.OrderByDescending(
                               r => r.Reviews.Average(rv => (double?)rv.Rating) ?? 0),
                _ => q.OrderBy(r => r.Title)
            };

            ViewBag.Categories = await _context.Recipes
                                        .Select(r => r.Category).Distinct().ToListAsync();
            return View(await q.ToListAsync());
        }
    }
}