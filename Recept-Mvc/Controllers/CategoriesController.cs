using Microsoft.AspNetCore.Mvc;
using Recept.Models;
using Recept.Services;

namespace Recept.Controllers;

/// <summary>
/// Sidor för kategorier och veckogränser. Logiken ligger i ICategoryService.
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetLimit(int id, int maxPerWeek) =>
        RunAndRedirect(() => _categories.SetWeeklyLimit(id, maxPerWeek), "Gränsen är sparad.", nameof(Index));

    // Först en sida som förklarar att recepten flyttas till "Övrigt", sedan tas kategorin bort.
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
