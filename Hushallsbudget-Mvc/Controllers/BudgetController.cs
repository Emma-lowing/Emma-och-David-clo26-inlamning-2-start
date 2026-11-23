using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hushallsbudget.Services;
using Hushallsbudget.ViewModels;

namespace Hushallsbudget.Controllers;

/// <summary>
/// Sidan för budget per kategori. Logiken ligger i IBudgetService.
/// </summary>
public class BudgetController : AppController
{
    private readonly IBudgetService _budgets;
    private readonly ICategoryService _categories;
    private readonly IMoneyFormatter _formatter;

    public BudgetController(IBudgetService budgets, ICategoryService categories, IMoneyFormatter formatter)
    {
        _budgets = budgets;
        _categories = categories;
        _formatter = formatter;
    }

    public IActionResult Index(int? year, int? month) => Run(() =>
    {
        var model = new BudgetViewModel();
        model.SelectMonth(year, month);
        model.Lines = _budgets.GetBudgetStatus(model.Year, model.Month)
            .Select(status => new BudgetLine
            {
                CategoryName = status.CategoryName,
                SpentText = _formatter.Format(status.Spent),
                BudgetText = _formatter.Format(status.BudgetAmount),
                Percent = status.Percent,
                Level = status.Level,
            })
            .ToList();
        model.Categories = _categories.GetAll()
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
            .ToList();
        return View(model);
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetBudget(int categoryId, int year, int month, decimal? amount)
    {
        if (!ModelState.IsValid || amount is null)
        {
            TempData["Error"] = "Budgeten måste vara ett tal, till exempel 1000.";
            return RedirectToAction(nameof(Index), new { year, month });
        }

        return RunAndRedirect(() => _budgets.SetBudget(categoryId, year, month, amount.Value),
            "Budgeten är sparad.", nameof(Index), new { year, month });
    }
}
