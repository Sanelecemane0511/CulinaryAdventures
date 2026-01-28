using CulinaryAdventures.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CulinaryAdventures.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 3;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Recipes.Any())
    {
        var recipes = new List<Recipe>
        {
            new() { Title = "Bobotie", Category = "Cape Malay", Ingredients = "500 g beef mince, 2 slices white bread, 1 onion, 2 tsp curry powder, 1 tsp turmeric, 2 tbsp apricot jam, 2 tbsp chutney, 1 cup milk, 2 eggs, bay leaves, salt & pepper.", Instructions = "1. Soak bread in milk. 2. Fry onion & spices, add mince. 3. Stir in jam, chutney, squeezed bread. 4. Bake 180 °C 45 min with egg-milk topping & bay leaves.", PrepTimeMin = 60, Servings = 4, ImageUrl = "/images/dishes/dish0.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Bunny Chow", Category = "Durban Indian", Ingredients = "1 hollowed-out white loaf, 300 g mutton curry, 2 potatoes, curry leaves, onion, garlic, ginger, spices.", Instructions = "Hollow loaf, ladle in hot curry, serve with carrot salad & coriander.", PrepTimeMin = 35, Servings = 2, ImageUrl = "/images/dishes/dish1.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Malva Pudding", Category = "Dessert", Ingredients = "1 cup sugar, 2 eggs, 1 tbsp apricot jam, 1 cup flour, 1 tsp bicarb, 1 cup milk, 1 tsp vinegar. Sauce: 1 cup cream, ½ cup butter, ½ cup sugar, ¼ cup hot water.", Instructions = "Cream sugar & eggs, add jam, fold in dry ingredients + milk/vinegar. Bake 180 °C 45 min. Pour hot sauce over immediately.", PrepTimeMin = 70, Servings = 6, ImageUrl = "/images/dishes/dish2.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Boerewors Roll", Category = "Street Food", Ingredients = "Fresh boerewors, Portuguese roll, tomato relish, mustard, caramelised onion.", Instructions = "Grill wors over coals till just done, tuck into toasted roll, top with relish & onion.", PrepTimeMin = 15, Servings = 1, ImageUrl = "/images/dishes/dish3.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Potjiekos", Category = "Traditional", Ingredients = "1 kg beef shin, baby potatoes, carrots, patty pans, onion, garlic, bay leaf, potjie spice, beef stock, red wine.", Instructions = "Layer meat & veg in cast-iron pot, add liquid, simmer 3–4 hours over low coals. Do NOT stir—shake gently.", PrepTimeMin = 240, Servings = 6, ImageUrl = "/images/dishes/dish4.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Chakalaka", Category = "Relish", Ingredients = "1 onion, 1 green pepper, 2 carrots, 1 tbsp curry powder, 1 can baked beans, 2 tomatoes, chilli.", Instructions = "Sauté veg, add spices & tomatoes, simmer 15 min, fold in beans. Serve cold or warm with pap.", PrepTimeMin = 25, Servings = 6, ImageUrl = "/images/dishes/dish5.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Pap & Sheba", Category = "Traditional", Ingredients = "2 cups maize meal, 4 cups water, salt. Sheba: 1 onion, 2 tomatoes, 1 green pepper, 1 tbsp tomato paste, sugar, herbs.", Instructions = "For pap: bring water to boil, add maize meal, stir till smooth, cover & steam 30 min. For sheba: sauté onion & pepper, add tomatoes & paste, simmer 20 min.", PrepTimeMin = 45, Servings = 4, ImageUrl = "/images/dishes/dish6.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Sosaties", Category = "Cape Malay", Ingredients = "500 g lamb cubes, 2 onions, 2 tbsp apricot jam, 2 tbsp curry powder, ¼ cup white vinegar, bay leaves.", Instructions = "Marinate meat overnight, skewer with onion & apricot, grill over coals till charred.", PrepTimeMin = 30, Servings = 4, ImageUrl = "/images/dishes/dish7.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Koeksisters", Category = "Dessert", Ingredients = "2 cups flour, 1 tbsp baking powder, ½ tsp salt, 2 tbsp butter, ¾ cup milk. Syrup: 2 cups sugar, 1 cup water, ½ tsp ginger, lemon juice.", Instructions = "Make dough, roll into braids, deep-fry till golden, plunge into ice-cold syrup.", PrepTimeMin = 60, Servings = 12, ImageUrl = "/images/dishes/dish8.jpg", OwnerName = "demo@ctu.co.za" },
            new() { Title = "Denningvleis", Category = "Cape Malay", Ingredients = "1 kg mutton ribs, 2 onions, 4 cloves, 4 all-spice, 2 bay leaves, ¼ cup tamarind, 2 tbsp brown sugar.", Instructions = "Brown ribs, add spices & tamarind, braise slowly 2 hours till sticky.", PrepTimeMin = 150, Servings = 4, ImageUrl = "/images/dishes/dish9.jpg", OwnerName = "demo@ctu.co.za" }
        };
        db.Recipes.AddRange(recipes);
        db.SaveChanges();
    }
}

app.Run();
