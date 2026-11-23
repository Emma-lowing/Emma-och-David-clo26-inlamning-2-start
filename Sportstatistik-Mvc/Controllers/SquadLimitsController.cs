using Microsoft.AspNetCore.Mvc;
using Sportstatistik.Models;
using Sportstatistik.Services;
using Sportstatistik.ViewModels;

namespace Sportstatistik.Controllers;

/// <summary>
/// Sidor för truppgräns per lag och säsong. Logiken ligger i ISquadLimitService.
/// </summary>
public class SquadLimitsController : AppController
{
    private readonly ISquadLimitService _squadLimits;
    private readonly ILeagueRepository _repository;

    public SquadLimitsController(ISquadLimitService squadLimits, ILeagueRepository repository)
    {
        _squadLimits = squadLimits;
        _repository = repository;
    }

    // Utan val visas innevarande säsong.
    public IActionResult Index(int? season) => Run(() =>
    {
        int shownSeason = Math.Clamp(season ?? DateTime.Today.Year, 2000, 2100);
        List<SquadLimit> limits = _squadLimits.GetLimits(shownSeason);

        return View(new SquadLimitsViewModel
        {
            Season = shownSeason,
            Rows = _repository.GetAllTeams()
                .Select(team => new SquadLimitRowViewModel
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    PlayerCount = _repository.GetPlayers(team.Id).Count,
                    MaxPlayers = limits.FirstOrDefault(l => l.TeamId == team.Id)?.MaxPlayers,
                })
                .ToList(),
        });
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetLimit(int teamId, int season, int? maxPlayers)
    {
        if (!ModelState.IsValid || maxPlayers is null or < 1 or > 100)
        {
            TempData["Error"] = "Truppgränsen måste vara ett heltal mellan 1 och 100.";
            return RedirectToAction(nameof(Index), new { season });
        }

        return RunAndRedirect(() => _squadLimits.SetLimit(teamId, season, maxPlayers.Value),
            "Truppgränsen är sparad.", nameof(Index), new { season });
    }
}
