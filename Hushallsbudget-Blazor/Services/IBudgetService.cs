using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Månadsbudget per kategori (funktion 5).
/// </summary>
public interface IBudgetService
{
    /// <summary>Hur mycket av budgeten som är använd, för varje kategori som har en budget den månaden.</summary>
    List<BudgetStatus> GetBudgetStatus(int year, int month);

    void SetBudget(int categoryId, int year, int month, decimal amount);
}
