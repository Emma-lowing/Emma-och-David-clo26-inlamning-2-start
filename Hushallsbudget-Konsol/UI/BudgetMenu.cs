using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.UI;

/// <summary>
/// Menyval för budget per kategori. Logiken ligger i IBudgetService.
/// </summary>
public class BudgetMenu
{
    private readonly IBudgetService _budgets;
    private readonly ICategoryService _categories;
    private readonly IMoneyFormatter _formatter;

    public BudgetMenu(IBudgetService budgets, ICategoryService categories, IMoneyFormatter formatter)
    {
        _budgets = budgets;
        _categories = categories;
        _formatter = formatter;
    }

    public void Run()
    {
        new Menu("Budget per kategori")
            .Add("1", "Visa budget för en månad", ShowBudget)
            .Add("2", "Sätt budget för en kategori", SetBudget)
            .Run();
    }

    private void ShowBudget()
    {
        (int year, int month) = MonthInput.Ask();
        List<BudgetStatus> statuses = _budgets.GetBudgetStatus(year, month);

        Console.WriteLine($"Budget för {MonthInput.Name(year, month)}");
        if (statuses.Count == 0)
        {
            Console.WriteLine("  Ingen kategori har en budget den månaden.");
            return;
        }

        foreach (BudgetStatus status in statuses)
        {
            Console.WriteLine($"  {status.CategoryName}: {_formatter.Format(status.Spent)} av {_formatter.Format(status.BudgetAmount)} ({status.Percent} %)");

            if (status.Level == BudgetLevel.Warning)
            {
                ConsoleInput.WriteWarning($"{status.CategoryName} är på {status.Percent} % av budgeten.");
            }
            else if (status.Level == BudgetLevel.Exceeded)
            {
                ConsoleInput.WriteWarning($"{status.CategoryName} är över budgeten ({status.Percent} %).");
            }
        }
    }

    private void SetBudget()
    {
        foreach (Category category in _categories.GetAll())
        {
            Console.WriteLine($"{category.Id}. {category.Name}");
        }

        int categoryId = ConsoleInput.ReadInt("Kategorins nummer", min: 1);
        (int year, int month) = MonthInput.Ask();
        decimal amount = ConsoleInput.ReadDecimal("Budget i kronor, till exempel 1000", min: 0);

        _budgets.SetBudget(categoryId, year, month, amount);
        Console.WriteLine("Budgeten är sparad.");
    }
}
