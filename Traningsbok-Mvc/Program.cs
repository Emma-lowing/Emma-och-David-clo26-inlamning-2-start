// Startpunkt för Traningsbok (MVC). Här registreras alla delar, och sedan startar webbservern.
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Traningsbok.Infrastructure;
using Traningsbok.Services;

var builder = WebApplication.CreateBuilder(args);

// Connectionstring läses från user-secrets (nyckeln DB_CONNECTION), aldrig från koden.
// I Development läser WebApplication user-secrets automatiskt.
string? connectionString = builder.Configuration["DB_CONNECTION"];

// Byt den här raden när ni har en riktig databas, till exempel:
//   builder.Services.AddSingleton<IWorkoutRepository>(new MongoWorkoutRepository(connectionString));
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Här passar till exempel mönstret Factory: en klass som väljer och skapar rätt repository
// utifrån konfigurationen, så att Program.cs inte behöver känna till varje databasklass.
// Låtsasdatabasen är Singleton så att listorna lever kvar mellan sidvisningarna.
builder.Services.AddSingleton<IWorkoutRepository, InMemoryWorkoutRepository>();

// De här fyra ska ni bygga. Tills dess visar sidorna demodata eller "Inte implementerat ännu".
builder.Services.AddSingleton<ITrainingTypeService, TrainingTypeService>();
builder.Services.AddSingleton<IGoalNotifier, GoalNotifier>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IWorkoutFormatter, WorkoutFormatter>();

builder.Services.AddControllersWithViews(options =>
{
    SwedishValidationMessages.Apply(options.ModelBindingMessageProvider);

    // Både "2,5" och "2.5" godkänns i formulären.
    options.ModelBinderProviders.Insert(0, new FlexibleDecimalModelBinderProvider());

    // Svenska fältnamn i felmeddelanden, till exempel "Längd i cm" i stället för "HeightCm".
    options.ModelMetadataDetailsProviders.Add(new SwedishDisplayNames());

    // Annars blir alla string-fält i modellerna obligatoriska, med engelska felmeddelanden.
    // Vilka fält som är obligatoriska bestäms i stället i controllern eller i era services.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

// Svenskt talformat i formulären: 62,5 i stället för 62.5.
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
