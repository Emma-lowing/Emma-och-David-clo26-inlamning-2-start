using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Månadsöversikten (funktion 4). En månad börjar alltid den första och slutar den sista (önskemål 2).
/// </summary>
public interface IMonthlyOverviewService
{
    MonthlyOverview GetOverview(int year, int month);
}
