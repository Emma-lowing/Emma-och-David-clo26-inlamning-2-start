using Microsoft.AspNetCore.Mvc;
using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.Controllers;

/// <summary>
/// Sidor för kategorier. Logiken ligger i ICategoryService.
/// </summary>
public class CategoriesController : AppController
{
    private readonly ICategoryService _categories;

    public CategoriesController(ICategoryService categories)
    {
        _categories = categories;
    }

    public IActionResult Index() => Run(() => View(_categories.GetAll()));

    // Ingen kontroll av tom text här: den regeln hör hemma i CategoryService.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string? name) =>
        RunAndRedirect(() => _categories.Add(name ?? ""), $"Kategorin \"{name}\" är tillagd.", nameof(Index));

    // Först en sida som förklarar att transaktionerna flyttas till "Övrigt", sedan tas kategorin bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        Category? category = _categories.GetAll().FirstOrDefault(c => c.Id == id);
        return category is null ? NotFound() : View(category);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _categories.Delete(id), "Kategorin är borttagen.", nameof(Index));
}
