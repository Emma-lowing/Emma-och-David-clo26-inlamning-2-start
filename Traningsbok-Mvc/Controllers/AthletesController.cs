using Microsoft.AspNetCore.Mvc;
using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.Controllers;

/// <summary>
/// Sidor för utövare.
/// </summary>
public class AthletesController : AppController
{
    private readonly IWorkoutRepository _repository;

    public AthletesController(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index() => Run(() => View(_repository.GetAllAthletes()));

    public IActionResult Create() => View(new Athlete());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Athlete athlete) => Run(() =>
    {
        if (string.IsNullOrWhiteSpace(athlete.FirstName))
        {
            ModelState.AddModelError(nameof(Athlete.FirstName), "Utövaren måste ha ett förnamn.");
        }
        if (!ModelState.IsValid)
        {
            return View(athlete);
        }

        try
        {
            _repository.AddAthlete(athlete);
            TempData["Message"] = $"Utövaren {athlete.FirstName} är tillagd.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(athlete);
        }
    });
}
