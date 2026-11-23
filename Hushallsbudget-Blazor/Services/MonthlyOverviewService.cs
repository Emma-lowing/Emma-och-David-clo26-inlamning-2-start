using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Ska byggas av er. GetOverview visar demodata så att sidan går att titta på.
/// </summary>
public class MonthlyOverviewService : IMonthlyOverviewService
{
    // DEMO: samma påhittade siffror varje månad. Ersätt med summor som räknas fram ur
    // månadens transaktioner, från den första till den sista i månaden (önskemål 2).
    // Siffrorna stämmer med låtsasdatabasens transaktioner för november 2026, så ni kan jämföra.
    public MonthlyOverview GetOverview(int year, int month) => new()
    {
        Year = year,
        Month = month,
        TotalIncome = 28500m,
        TotalExpenses = 13538.5m,
        Balance = 14961.5m,
        CategoryTotals =
        [
            new CategoryTotal { CategoryName = "Boende", Amount = 9800m },
            new CategoryTotal { CategoryName = "Mat", Amount = 1249.5m },
            new CategoryTotal { CategoryName = "El och internet", Amount = 899m },
            new CategoryTotal { CategoryName = "Nöje", Amount = 790m },
            new CategoryTotal { CategoryName = "Transport", Amount = 450m },
            new CategoryTotal { CategoryName = "Hälsa", Amount = 350m },
        ],
    };
}
