using Microsoft.AspNetCore.Mvc;
using Recept.Models;
using Recept.Services;

namespace Recept.Controllers;

/// <summary>
/// Sidor för kockar.
/// </summary>
public class ChefsController : AppController
{
    private readonly IRecipeRepository _repository;

    public ChefsController(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index() => Run(() => View(_repository.GetAllChefs()));

    public IActionResult Create() => View(new Chef());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Chef chef) => Run(() =>
    {
        if (string.IsNullOrWhiteSpace(chef.FirstName))
        {
            ModelState.AddModelError(nameof(Chef.FirstName), "Kocken måste ha ett förnamn.");
        }
        if (!ModelState.IsValid)
        {
            return View(chef);
        }

        try
        {
            _repository.AddChef(chef);
            TempData["Message"] = $"Kocken {chef.FirstName} är tillagd.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(chef);
        }
    });
}
