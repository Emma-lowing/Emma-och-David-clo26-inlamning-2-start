// Startpunkt för Recept (Blazor). Här registreras alla delar, och sedan startar webbservern.
using System.Globalization;
using Recept.Components;
using Recept.Services;

// Svenskt talformat och svenska datum: 2,5 i stället för 2.5.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("sv-SE");

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

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
