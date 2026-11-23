using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sportstatistik.Models;
using Sportstatistik.Services;
using Sportstatistik.ViewModels;

namespace Sportstatistik.Controllers;

/// <summary>
/// Sidor för matcher. Controllern sköter bara formulär och sidor;
/// lagring går via ILeagueRepository och formatering via IResultFormatter.
/// </summary>
public class MatchesController : AppController
{
    private readonly ILeagueRepository _repository;
    private readonly IResultFormatter _formatter;

    public MatchesController(ILeagueRepository repository, IResultFormatter formatter)
    {
        _repository = repository;
        _formatter = formatter;
    }

    // Du behöver bygga ut detta mer: sortera på datum när Match har ett datum.
    public IActionResult Index() => Run(() => View(_repository.GetAllMatches().Select(ToRow).ToList()));

    public IActionResult Create() => Run(() =>
    {
        ViewData["Teams"] = TeamOptions();
        return View(new MatchFormModel());
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MatchFormModel form) => Run(() =>
    {
        if (_repository.GetTeam(form.HomeTeamId) is null)
        {
            ModelState.AddModelError(nameof(MatchFormModel.HomeTeamId), "Välj ett hemmalag.");
        }
        if (_repository.GetTeam(form.AwayTeamId) is null)
        {
            ModelState.AddModelError(nameof(MatchFormModel.AwayTeamId), "Välj ett bortalag.");
        }
        if (!ModelState.IsValid)
        {
            ViewData["Teams"] = TeamOptions();
            return View(form);
        }

        try
        {
            // Ingen kontroll av att ett lag möter sig självt här: den regeln hör hemma i repository eller service.
            Match match = form.ToMatch();
            _repository.AddMatch(match);
            TempData["Message"] = $"Matchen är sparad: {ToRow(match).Title}, {_formatter.FormatResult(match.HomeGoals, match.AwayGoals)}.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            // Felaktig inmatning som upptäcks i era services visas i formuläret.
            ModelState.AddModelError("", ex.Message);
            ViewData["Teams"] = TeamOptions();
            return View(form);
        }
    });

    // Önskemål 5: först en sida som frågar "Är du säker?", sedan tas matchen bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        Match? match = _repository.GetMatch(id);
        return match is null ? NotFound() : View(ToRow(match));
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _repository.DeleteMatch(id), "Matchen är borttagen.", nameof(Index));

    private MatchRowViewModel ToRow(Match match) => new()
    {
        Id = match.Id,
        HomeTeam = TeamName(match.HomeTeamId),
        AwayTeam = TeamName(match.AwayTeamId),
        Result = _formatter.FormatResult(match.HomeGoals, match.AwayGoals),
    };

    private string TeamName(int teamId) => _repository.GetTeam(teamId)?.Name ?? "(okänt lag)";

    private List<SelectListItem> TeamOptions() =>
        _repository.GetAllTeams()
            .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            .ToList();
}
