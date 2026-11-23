using Microsoft.AspNetCore.Mvc;
using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.Controllers;

/// <summary>
/// Sidor för hushållet och dess medlemmar.
/// </summary>
public class HouseholdController : AppController
{
    private readonly IHouseholdRepository _repository;

    public HouseholdController(IHouseholdRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index() => Run(() =>
    {
        ViewData["HouseholdName"] = _repository.GetHousehold().Name;
        return View(_repository.GetAllMembers());
    });

    public IActionResult AddMember() => View(new Member());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddMember(Member member) => Run(() =>
    {
        if (string.IsNullOrWhiteSpace(member.FirstName))
        {
            ModelState.AddModelError(nameof(Member.FirstName), "Medlemmen måste ha ett förnamn.");
        }
        if (!ModelState.IsValid)
        {
            return View(member);
        }

        try
        {
            _repository.AddMember(member);
            TempData["Message"] = $"{member.FirstName} är tillagd i hushållet.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(member);
        }
    });
}
