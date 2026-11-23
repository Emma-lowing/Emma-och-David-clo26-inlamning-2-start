using Microsoft.AspNetCore.Mvc;
using Hushallsbudget.Models;
using Hushallsbudget.Services;
using Hushallsbudget.ViewModels;

namespace Hushallsbudget.Controllers;

/// <summary>
/// Sidan för månadsöversikten. Logiken ligger i IMonthlyOverviewService.
/// </summary>
public class OverviewController : AppController
{
    private readonly IMonthlyOverviewService _overview;
    private readonly IMoneyFormatter _formatter;

    public OverviewController(IMonthlyOverviewService overview, IMoneyFormatter formatter)
    {
        _overview = overview;
        _formatter = formatter;
    }

    public IActionResult Index(int? year, int? month) => Run(() =>
    {
        var model = new OverviewViewModel();
        model.SelectMonth(year, month);

        MonthlyOverview overview = _overview.GetOverview(model.Year, model.Month);
        model.IncomeText = _formatter.Format(overview.TotalIncome);
        model.ExpensesText = _formatter.Format(overview.TotalExpenses);
        model.BalanceText = _formatter.Format(overview.Balance);
        model.CategoryLines = overview.CategoryTotals
            .Select(total => (total.CategoryName, _formatter.Format(total.Amount)))
            .ToList();

        return View(model);
    });
}
