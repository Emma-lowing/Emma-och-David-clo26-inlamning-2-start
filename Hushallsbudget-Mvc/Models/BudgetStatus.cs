namespace Hushallsbudget.Models;

/// <summary>
/// Hur mycket av en kategoris månadsbudget som är använd.
/// </summary>
public class BudgetStatus
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public decimal BudgetAmount { get; set; }
    public decimal Spent { get; set; }

    /// <summary>Hur stor del av budgeten som är använd, i procent. 81 betyder 81 %.</summary>
    public int Percent { get; set; }

    public BudgetLevel Level { get; set; }
}
