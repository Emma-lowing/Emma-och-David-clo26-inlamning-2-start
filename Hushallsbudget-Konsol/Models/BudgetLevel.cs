namespace Hushallsbudget.Models;

/// <summary>
/// Läget för en kategoris budget. Gränserna är 80 % och 100 % (funktion 6).
/// </summary>
public enum BudgetLevel
{
    /// <summary>Under 80 % av budgeten.</summary>
    Ok,

    /// <summary>Över 80 % av budgeten.</summary>
    Warning,

    /// <summary>Över 100 % av budgeten.</summary>
    Exceeded,
}
