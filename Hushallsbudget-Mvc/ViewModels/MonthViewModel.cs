namespace Hushallsbudget.ViewModels;

/// <summary>
/// En vald månad. Basklass för sidor som visar en månad i taget:
/// transaktioner, månadsöversikt och budget.
/// </summary>
public class MonthViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }

    /// <summary>Till exempel "november 2026".</summary>
    public string MonthName => new DateOnly(Year, Month, 1).ToString("MMMM yyyy");

    public DateOnly PreviousMonth => new DateOnly(Year, Month, 1).AddMonths(-1);
    public DateOnly NextMonth => new DateOnly(Year, Month, 1).AddMonths(1);

    /// <summary>Utan val visas innevarande månad. Ogiltiga värden flyttas in till närmaste giltiga.</summary>
    public void SelectMonth(int? year, int? month)
    {
        DateTime today = DateTime.Today;
        Year = Math.Clamp(year ?? today.Year, 2000, 2100);
        Month = Math.Clamp(month ?? today.Month, 1, 12);
    }
}
