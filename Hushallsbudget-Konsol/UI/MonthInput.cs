namespace Hushallsbudget.UI;

/// <summary>
/// Val av månad. Används av transaktioner, översikt och budget.
/// </summary>
public static class MonthInput
{
    public static (int Year, int Month) Ask()
    {
        int year = ConsoleInput.ReadInt("År", min: 2000, max: 2100);
        int month = ConsoleInput.ReadInt("Månad (1–12)", min: 1, max: 12);
        return (year, month);
    }

    /// <summary>Till exempel "november 2026".</summary>
    public static string Name(int year, int month) => new DateOnly(year, month, 1).ToString("MMMM yyyy");
}
