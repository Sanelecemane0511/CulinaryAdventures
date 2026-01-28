using CulinaryAdventures.Data;
using CulinaryAdventures.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace CulinaryAdventures.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public RecipesController(ApplicationDbContext ctx) => _ctx = ctx;

        // ----------  READ  ----------
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _ctx.Recipes
                                   .Include(r => r.Reviews)
                                   .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return NotFound();
            return View(recipe);
        }

        // ----------  CREATE  ----------
        [Authorize]
        public IActionResult Submit() => View();

        [HttpPost][Authorize]
        public async Task<IActionResult> Submit(Recipe r)
        {
            if (!ModelState.IsValid) return View(r);

            r.OwnerName = User.Identity!.Name!;
            r.Created   = DateTime.UtcNow;
            _ctx.Recipes.Add(r);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Details", new { id = r.Id });
        }

        // ----------  UPDATE  ----------
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _ctx.Recipes.FindAsync(id);
            if (r == null || r.OwnerName != User.Identity!.Name) return Forbid();
            return View(r);
        }

        [HttpPost][Authorize]
        public async Task<IActionResult> Edit(Recipe r)
        {
            var orig = await _ctx.Recipes.AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.Id == r.Id);
            if (orig == null || orig.OwnerName != User.Identity!.Name) return Forbid();

            if (!ModelState.IsValid) return View(r);

            r.OwnerName = orig.OwnerName;
            _ctx.Recipes.Update(r);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Details", new { id = r.Id });
        }

        // ----------  DELETE  ----------
        [HttpPost][Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _ctx.Recipes.FindAsync(id);
            if (r == null || r.OwnerName != User.Identity!.Name) return Forbid();

            _ctx.Recipes.Remove(r);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}