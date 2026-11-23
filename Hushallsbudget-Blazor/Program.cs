// Startpunkt för Hushallsbudget (Blazor). Här registreras alla delar, och sedan startar webbservern.
using System.Globalization;
using Hushallsbudget.Components;
using Hushallsbudget.Services;

// Svenskt talformat och svenska datum: 249,50 i stället för 249.50.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("sv-SE");

var builder = WebApplication.CreateBuilder(args);

// Connectionstring läses från user-secrets (nyckeln DB_CONNECTION), aldrig från koden.
// I Development läser WebApplication user-secrets automatiskt.
string? connectionString = builder.Configuration["DB_CONNECTION"];

// Byt den här raden när ni har en riktig databas, till exempel:
//   builder.Services.AddSingleton<IHouseholdRepository>(new MongoHouseholdRepository(connectionString));
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Här passar till exempel mönstret Factory: en klass som väljer och skapar rätt repository
// utifrån konfigurationen, så att Program.cs inte behöver känna till varje databasklass.
// Låtsasdatabasen är Singleton så att listorna lever kvar mellan sidvisningarna.
builder.Services.AddSingleton<IHouseholdRepository, InMemoryHouseholdRepository>();

// De här ska ni bygga. Tills dess visar sidorna demodata eller "Inte implementerat ännu".
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IBudgetService, BudgetService>();
builder.Services.AddSingleton<IBudgetWarningService, BudgetWarningService>();
builder.Services.AddSingleton<IMonthlyOverviewService, MonthlyOverviewService>();
builder.Services.AddSingleton<IMoneyFormatter, MoneyFormatter>();

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
