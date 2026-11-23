using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Ska byggas av er. GetBudgetStatus visar demodata så att sidan går att titta på.
/// SetBudget kastar NotImplementedException, och sidan visar då "Inte implementerat ännu".
/// </summary>
public class BudgetService : IBudgetService
{
    // DEMO: samma påhittade siffror varje månad. Ersätt med budgetar från databasen och
    // summor som räknas fram ur månadens transaktioner.
    // "Spent" stämmer med låtsasdatabasens transaktioner för november 2026, så ni kan jämföra.
    public List<BudgetStatus> GetBudgetStatus(int year, int month) =>
    [
        new BudgetStatus { CategoryId = 1, CategoryName = "Boende", BudgetAmount = 12000m, Spent = 9800m, Percent = 81, Level = BudgetLevel.Warning },
        new BudgetStatus { CategoryId = 2, CategoryName = "Mat", BudgetAmount = 4000m, Spent = 1249.5m, Percent = 31, Level = BudgetLevel.Ok },
        new BudgetStatus { CategoryId = 3, CategoryName = "Transport", BudgetAmount = 600m, Spent = 450m, Percent = 75, Level = BudgetLevel.Ok },
        new BudgetStatus { CategoryId = 4, CategoryName = "El och internet", BudgetAmount = 800m, Spent = 899m, Percent = 112, Level = BudgetLevel.Exceeded },
        new BudgetStatus { CategoryId = 5, CategoryName = "Nöje", BudgetAmount = 1000m, Spent = 790m, Percent = 79, Level = BudgetLevel.Ok },
    ];

    // Ett belopp som inte är större än noll ska ge ett tydligt felmeddelande.
    // Kasta ArgumentException, så visar sidan texten.
    public void SetBudget(int categoryId, int year, int month, decimal amount) => throw new NotImplementedException();
}
