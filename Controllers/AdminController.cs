using CulinaryAdventures.Data;
using CulinaryAdventures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CulinaryAdventures.Controllers
{
    [Authorize]   // any logged-in user
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public AdminController(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.Name != "demo@ctu.co.za")
                return Forbid();

            ViewBag.Recipes = await _ctx.Recipes.Include(r => r.Reviews).ToListAsync();
            ViewBag.Users   = await _ctx.Users.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            if (User.Identity?.Name != "demo@ctu.co.za") return Forbid();

            var r = await _ctx.Recipes.FindAsync(id);
            if (r != null) _ctx.Recipes.Remove(r);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (User.Identity?.Name != "demo@ctu.co.za") return Forbid();

            var u = await _ctx.Users.FindAsync(id);
            if (u != null) _ctx.Users.Remove(u);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}