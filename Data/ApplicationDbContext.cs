using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CulinaryAdventures.Models;

namespace CulinaryAdventures.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Review> Reviews => Set<Review>();
}
