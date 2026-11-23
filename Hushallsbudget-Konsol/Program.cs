// Startpunkt för Hushallsbudget (konsol). Här kopplas alla delar ihop, och sedan startar menyn.
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using Hushallsbudget.Services;
using Hushallsbudget.UI;

// Svenska tecken och svenskt talformat (249,50 i stället för 249.50).
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");

// Connectionstring läses från user-secrets, aldrig från koden.
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? connectionString = config["DB_CONNECTION"];

// Byt den här raden när ni har en riktig databas, till exempel:
//   IHouseholdRepository repository = new MongoHouseholdRepository(connectionString);
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Här passar till exempel mönstret Factory: en klass som väljer och skapar rätt repository
// utifrån konfigurationen, så att Program.cs inte behöver känna till varje databasklass.
IHouseholdRepository repository = new InMemoryHouseholdRepository();

// De här ska ni bygga. Tills dess visar menyn demodata eller "Inte implementerat ännu".
ICategoryService categories = new CategoryService();
IBudgetService budgets = new BudgetService();
IBudgetWarningService warnings = new BudgetWarningService();
IMonthlyOverviewService overview = new MonthlyOverviewService();
IMoneyFormatter formatter = new MoneyFormatter();

var transactionMenu = new TransactionMenu(repository, categories, warnings, formatter);
var overviewMenu = new OverviewMenu(overview, formatter);
var householdMenu = new HouseholdMenu(repository);
var categoryMenu = new CategoryMenu(categories);
var budgetMenu = new BudgetMenu(budgets, categories, formatter);

Console.WriteLine("Välkommen till Hushållsbudget!");

new Menu("Hushållsbudget", exitLabel: "Avsluta")
    .Add("1", "Visa transaktioner för en månad", transactionMenu.ListTransactions)
    .Add("2", "Lägg till transaktion", transactionMenu.AddTransaction)
    .Add("3", "Ta bort transaktion", transactionMenu.DeleteTransaction)
    .Add("4", "Visa månadsöversikt", overviewMenu.ShowMonthlyOverview)
    .Add("5", "Lägg till medlem i hushållet", householdMenu.AddMember)
    .Add("6", "Visa hushållet", householdMenu.ShowHousehold)
    .Add("7", "Kategorier", categoryMenu.Run)
    .Add("8", "Budget per kategori", budgetMenu.Run)
    .Run();

Console.WriteLine("Hej då!");
