using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hushallsbudget.Models;
using Hushallsbudget.Services;
using Hushallsbudget.ViewModels;

namespace Hushallsbudget.Controllers;

/// <summary>
/// Sidor för transaktioner. Controllern sköter bara formulär och sidor;
/// lagring går via IHouseholdRepository och budgetvarningar via IBudgetWarningService.
/// </summary>
public class TransactionsController : AppController
{
    private readonly IHouseholdRepository _repository;
    private readonly ICategoryService _categories;
    private readonly IBudgetWarningService _warnings;
    private readonly IMoneyFormatter _formatter;

    public TransactionsController(IHouseholdRepository repository, ICategoryService categories,
        IBudgetWarningService warnings, IMoneyFormatter formatter)
    {
        _repository = repository;
        _categories = categories;
        _warnings = warnings;
        _formatter = formatter;
    }

    public IActionResult Index(int? year, int? month) => Run(() =>
    {
        var model = new TransactionListViewModel();
        model.SelectMonth(year, month);
        model.Transactions = _repository.GetTransactionsForMonth(model.Year, model.Month)
            .Select(t => new TransactionLine
            {
                Id = t.Id,
                Date = t.Date,
                Category = t.Category,
                Note = t.Note,
                AmountText = _formatter.Format(t.Amount),
            })
            .ToList();
        return View(model);
    });

    public IActionResult Create() => Run(() =>
    {
        ViewData["Categories"] = CategoryOptions();
        return View(new TransactionFormModel());
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TransactionFormModel form) => Run(() =>
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = CategoryOptions();
            return View(form);
        }

        try
        {
            Transaction transaction = form.ToTransaction();
            _repository.AddTransaction(transaction);
            TempData["Message"] = "Transaktionen är sparad.";

            string? warnings = BudgetWarnings(transaction);
            if (warnings is not null)
            {
                TempData["Warning"] = warnings;
            }

            return RedirectToAction(nameof(Index), new { year = transaction.Date.Year, month = transaction.Date.Month });
        }
        catch (ArgumentException ex)
        {
            // Felaktig inmatning som upptäcks i era services visas i formuläret.
            ModelState.AddModelError("", ex.Message);
            ViewData["Categories"] = CategoryOptions();
            return View(form);
        }
    });

    // Önskemål 5: först en sida som frågar "Är du säker?", sedan tas transaktionen bort.
    // "Nej" lämnar allt oförändrat.
    public IActionResult Delete(int id) => Run(() =>
    {
        Transaction? transaction = _repository.GetTransaction(id);
        if (transaction is null)
        {
            return NotFound();
        }

        ViewData["AmountText"] = _formatter.Format(transaction.Amount);
        return View(transaction);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) => Run(() =>
    {
        Transaction? transaction = _repository.GetTransaction(id);
        if (transaction is null)
        {
            return NotFound();
        }

        return RunAndRedirect(() => _repository.DeleteTransaction(id), "Transaktionen är borttagen.",
            nameof(Index), new { year = transaction.Date.Year, month = transaction.Date.Month });
    });

    // Transaktionen är redan sparad när varningarna kontrolleras. Om varningarna inte är byggda än
    // visas en upplysning i stället för sidan "Inte implementerat ännu".
    private string? BudgetWarnings(Transaction transaction)
    {
        try
        {
            List<string> warnings = _warnings.CheckBudget(transaction);
            return warnings.Count > 0 ? string.Join(" ", warnings) : null;
        }
        catch (NotImplementedException)
        {
            return "Budgetvarningar är inte implementerade ännu. Dem ska ni bygga i BudgetWarningService.";
        }
    }

    private List<SelectListItem> CategoryOptions() =>
        _categories.GetAll()
            .Select(c => new SelectListItem(c.Name, c.Name))
            .ToList();
}
