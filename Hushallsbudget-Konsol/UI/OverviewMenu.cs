using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.UI;

/// <summary>
/// Menyval för månadsöversikten. Logiken ligger i IMonthlyOverviewService.
/// </summary>
public class OverviewMenu
{
    private readonly IMonthlyOverviewService _overview;
    private readonly IMoneyFormatter _formatter;

    public OverviewMenu(IMonthlyOverviewService overview, IMoneyFormatter formatter)
    {
        _overview = overview;
        _formatter = formatter;
    }

    public void ShowMonthlyOverview()
    {
        (int year, int month) = MonthInput.Ask();
        MonthlyOverview overview = _overview.GetOverview(year, month);

        Console.WriteLine();
        Console.WriteLine($"Månadsöversikt för {MonthInput.Name(year, month)}");
        Console.WriteLine($"  Inkomster: {_formatter.Format(overview.TotalIncome)}");
        Console.WriteLine($"  Utgifter:  {_formatter.Format(overview.TotalExpenses)}");
        Console.WriteLine($"  Saldo:     {_formatter.Format(overview.Balance)}");

        Console.WriteLine("Utgifter per kategori:");
        if (overview.CategoryTotals.Count == 0)
        {
            Console.WriteLine("  Inga utgifter den månaden.");
        }
        foreach (CategoryTotal total in overview.CategoryTotals)
        {
            Console.WriteLine($"  {total.CategoryName}: {_formatter.Format(total.Amount)}");
        }
    }
}
