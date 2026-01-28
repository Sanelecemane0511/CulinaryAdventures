using CulinaryAdventures.Data;
using CulinaryAdventures.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CulinaryAdventures.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public ReviewsController(ApplicationDbContext ctx) => _ctx = ctx;

        [HttpPost]  
        public async Task<IActionResult> AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Created = System.DateTime.UtcNow;
                _ctx.Reviews.Add(review);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Recipes", new { id = review.RecipeId });
        }
    }
}