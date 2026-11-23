namespace Hushallsbudget.Models;

/// <summary>
/// Månadsöversikten: summor för en månad, från den första till den sista (önskemål 2).
/// Utgifterna är positiva belopp här (önskemål 1).
/// </summary>
public class MonthlyOverview
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance { get; set; }

    /// <summary>Summa utgifter per kategori.</summary>
    public List<CategoryTotal> CategoryTotals { get; set; } = new();
}
