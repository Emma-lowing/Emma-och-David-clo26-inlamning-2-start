using Microsoft.AspNetCore.Mvc;
using Sportstatistik.Services;
using Sportstatistik.ViewModels;

namespace Sportstatistik.Controllers;

/// <summary>
/// Seriens tabell för en säsong. Tabellen räknas fram i IStandingsService, här visas den bara.
/// </summary>
public class StandingsController : AppController
{
    private readonly IStandingsService _standings;
    private readonly IResultFormatter _formatter;

    public StandingsController(IStandingsService standings, IResultFormatter formatter)
    {
        _standings = standings;
        _formatter = formatter;
    }

    // Utan val visas innevarande säsong. En säsong följer kalenderåret (önskemål 2).
    public IActionResult Index(int? season) => Run(() =>
    {
        int shownSeason = Math.Clamp(season ?? DateTime.Today.Year, 2000, 2100);
        int place = 1;

        return View(new StandingsViewModel
        {
            Season = shownSeason,
            Lines = _standings.GetStandings(shownSeason)
                .Select(row => new StandingLineViewModel
                {
                    Place = place++,
                    TeamName = row.TeamName,
                    Played = row.Played,
                    Won = row.Won,
                    Drawn = row.Drawn,
                    Lost = row.Lost,
                    GoalsFor = row.GoalsFor,
                    GoalsAgainst = row.GoalsAgainst,
                    GoalDifference = _formatter.FormatGoalDifference(row.GoalDifference),
                    Points = row.Points,
                })
                .ToList(),
        });
    });
}
