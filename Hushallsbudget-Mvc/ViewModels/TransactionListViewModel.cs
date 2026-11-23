namespace Hushallsbudget.ViewModels;

/// <summary>
/// Månadens transaktioner, med beloppen redan formaterade.
/// </summary>
public class TransactionListViewModel : MonthViewModel
{
    public List<TransactionLine> Transactions { get; set; } = new();
}

public class TransactionLine
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Category { get; set; } = "";
    public string Note { get; set; } = "";
    public string AmountText { get; set; } = "";
}
