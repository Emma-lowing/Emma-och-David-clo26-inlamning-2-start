using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sportstatistik.Models;
using Sportstatistik.Services;
using Sportstatistik.ViewModels;

namespace Sportstatistik.Controllers;

/// <summary>
/// Sidor för lag och spelare. Controllern sköter bara formulär och sidor;
/// lagring går via ILeagueRepository och truppgränsen via ISquadLimitService.
/// </summary>
public class TeamsController : AppController
{
    private readonly ILeagueRepository _repository;
    private readonly IPositionService _positions;
    private readonly ISquadLimitService _squadLimits;

    public TeamsController(ILeagueRepository repository, IPositionService positions, ISquadLimitService squadLimits)
    {
        _repository = repository;
        _positions = positions;
        _squadLimits = squadLimits;
    }

    public IActionResult Index() => Run(() =>
        View(_repository.GetAllTeams()
            .Select(t => new TeamSummaryViewModel { Id = t.Id, Name = t.Name, PlayerCount = _repository.GetPlayers(t.Id).Count })
            .ToList()));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Laget måste ha ett namn.";
            return RedirectToAction(nameof(Index));
        }

        string trimmed = name.Trim();
        return RunAndRedirect(() => _repository.AddTeam(new Team { Name = trimmed }), $"Laget \"{trimmed}\" är tillagt.", nameof(Index));
    }

    public IActionResult Details(int id) => Run(() =>
    {
        Team? team = _repository.GetTeam(id);
        if (team is null)
        {
            return NotFound();
        }

        return View(new TeamDetailsViewModel { Team = team, Players = _repository.GetPlayers(team.Id) });
    });

    public IActionResult AddPlayer(int teamId) => Run(() =>
    {
        Team? team = _repository.GetTeam(teamId);
        if (team is null)
        {
            return NotFound();
        }

        SetPlayerFormData(team);
        return View(new Player { TeamId = team.Id });
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddPlayer(Player player) => Run(() =>
    {
        Team? team = _repository.GetTeam(player.TeamId);
        if (team is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(player.FirstName))
        {
            ModelState.AddModelError(nameof(Player.FirstName), "Spelaren måste ha ett förnamn.");
        }
        if (string.IsNullOrWhiteSpace(player.Position))
        {
            ModelState.AddModelError(nameof(Player.Position), "Välj en position.");
        }
        if (!ModelState.IsValid)
        {
            SetPlayerFormData(team);
            return View(player);
        }

        try
        {
            _repository.AddPlayer(player);
            TempData["Message"] = $"{player.FirstName} är tillagd i {team.Name}.";
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            SetPlayerFormData(team);
            return View(player);
        }

        // Funktion 6: varning vid 80 % och 100 % av truppgränsen. Säsongen är innevarande år.
        // Spelaren är redan sparad, så här visas bara en varning om tjänsten inte är byggd än.
        try
        {
            List<string> warnings = _squadLimits.CheckSquadLimit(team.Id, DateTime.Today.Year);
            if (warnings.Count > 0)
            {
                TempData["Warning"] = string.Join(" ", warnings);
            }
        }
        catch (NotImplementedException)
        {
            TempData["Warning"] = "Varningen vid truppgränsen är inte implementerad ännu. Den ska ni bygga i SquadLimitService.";
        }

        return RedirectToAction(nameof(Details), new { id = team.Id });
    });

    private void SetPlayerFormData(Team team)
    {
        ViewData["TeamName"] = team.Name;
        ViewData["Positions"] = _positions.GetAll()
            .Select(p => new SelectListItem(p.Name, p.Name))
            .ToList();
    }
}
