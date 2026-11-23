using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Varningar när en kategori går över 80 % och 100 % av sin budget (funktion 6).
/// </summary>
public interface IBudgetWarningService
{
    /// <summary>
    /// Anropas efter att en transaktion har sparats. Returnerar de varningar användaren ska se,
    /// till exempel "Nöje är på 81 % av budgeten". Tom lista betyder inga varningar.
    /// </summary>
    List<string> CheckBudget(Transaction transaction);
}
