namespace Hushallsbudget.ViewModels;

/// <summary>
/// Månadsöversikten, med beloppen redan formaterade.
/// </summary>
public class OverviewViewModel : MonthViewModel
{
    public string IncomeText { get; set; } = "";
    public string ExpensesText { get; set; } = "";
    public string BalanceText { get; set; } = "";

    /// <summary>Utgifter per kategori.</summary>
    public List<(string CategoryName, string AmountText)> CategoryLines { get; set; } = new();
}
