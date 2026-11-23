using Microsoft.AspNetCore.Mvc;
using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.Controllers;

/// <summary>
/// Sidor för träningstyper och veckomål. Logiken ligger i ITrainingTypeService.
/// </summary>
public class TrainingTypesController : AppController
{
    private readonly ITrainingTypeService _trainingTypes;

    public TrainingTypesController(ITrainingTypeService trainingTypes)
    {
        _trainingTypes = trainingTypes;
    }

    public IActionResult Index() => Run(() => View(_trainingTypes.GetAll()));

    // Ingen kontroll av tom text här: den regeln hör hemma i TrainingTypeService.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string? name) =>
        RunAndRedirect(() => _trainingTypes.Add(name ?? ""), $"Träningstypen \"{name}\" är tillagd.", nameof(Index));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetGoal(int id, int? minutes)
    {
        if (minutes is null or < 1 or > 10080)
        {
            TempData["Error"] = "Veckomålet måste vara ett heltal mellan 1 och 10080 minuter.";
            return RedirectToAction(nameof(Index));
        }

        return RunAndRedirect(() => _trainingTypes.SetWeeklyGoal(id, minutes.Value), "Veckomålet är sparat.", nameof(Index));
    }

    // Först en sida som förklarar att passen flyttas till "Övrigt", sedan tas träningstypen bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        TrainingType? trainingType = _trainingTypes.GetAll().FirstOrDefault(t => t.Id == id);
        return trainingType is null ? NotFound() : View(trainingType);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _trainingTypes.Delete(id), "Träningstypen är borttagen.", nameof(Index));
}
