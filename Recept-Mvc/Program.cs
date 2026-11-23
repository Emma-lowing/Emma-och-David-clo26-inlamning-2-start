// Startpunkt för Recept (MVC). Här registreras alla delar, och sedan startar webbservern.
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Recept.Infrastructure;
using Recept.Services;

var builder = WebApplication.CreateBuilder(args);

// Connectionstring läses från user-secrets (nyckeln DB_CONNECTION), aldrig från koden.
// I Development läser WebApplication user-secrets automatiskt.
string? connectionString = builder.Configuration["DB_CONNECTION"];

// Byt den här raden när ni har en riktig databas, till exempel:
//   builder.Services.AddSingleton<IRecipeRepository>(new MongoRecipeRepository(connectionString));
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Låtsasdatabasen är Singleton så att listorna lever kvar mellan sidvisningarna.
builder.Services.AddSingleton<IRecipeRepository, InMemoryRecipeRepository>();

// De här tre ska ni bygga. Tills dess visar sidorna "Inte implementerat ännu".
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IMealPlanService, MealPlanService>();
builder.Services.AddSingleton<IRecipeCalculator, RecipeCalculator>();

builder.Services.AddControllersWithViews(options =>
{
    SwedishValidationMessages.Apply(options.ModelBindingMessageProvider);

    // Annars blir alla string-fält i modellerna obligatoriska, med engelska felmeddelanden.
    // Vilka fält som är obligatoriska bestäms i stället i controllern eller i era services.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

// Svenskt talformat i formulären: 2,5 i stället för 2.5.
var swedish = new CultureInfo("sv-SE");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(swedish),
    SupportedCultures = [swedish],
    SupportedUICultures = [swedish],
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
