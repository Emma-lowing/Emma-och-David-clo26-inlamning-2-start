using Microsoft.AspNetCore.Mvc;
using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.Controllers;

/// <summary>
/// Sidor för spelarpositioner. Logiken ligger i IPositionService.
/// </summary>
public class PositionsController : AppController
{
    private readonly IPositionService _positions;

    public PositionsController(IPositionService positions)
    {
        _positions = positions;
    }

    public IActionResult Index() => Run(() => View(_positions.GetAll()));

    // Ingen kontroll av tom text här: den regeln hör hemma i PositionService.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string? name) =>
        RunAndRedirect(() => _positions.Add(name ?? ""), $"Positionen \"{name}\" är tillagd.", nameof(Index));

    // Först en sida som förklarar att spelarna flyttas till "Övrigt", sedan tas positionen bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        Position? position = _positions.GetAll().FirstOrDefault(p => p.Id == id);
        return position is null ? NotFound() : View(position);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _positions.Delete(id), "Positionen är borttagen.", nameof(Index));
}
