using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.UI;

/// <summary>
/// Menyval för seriens tabell. Tabellen räknas fram i IStandingsService, här skrivs den bara ut.
/// </summary>
public class StandingsMenu
{
    private readonly IStandingsService _standings;
    private readonly IResultFormatter _formatter;

    public StandingsMenu(IStandingsService standings, IResultFormatter formatter)
    {
        _standings = standings;
        _formatter = formatter;
    }

    public void ShowStandings()
    {
        int season = ConsoleInput.ReadInt("Säsong (år)", min: 2000, max: 2100);
        List<StandingRow> rows = _standings.GetStandings(season);

        Console.WriteLine();
        Console.WriteLine($"Tabell för säsong {season}");
        if (rows.Count == 0)
        {
            Console.WriteLine("Inga matcher är spelade den här säsongen.");
            return;
        }

        Console.WriteLine($"{"#",-3}{"Lag",-20}{"S",4}{"V",4}{"O",4}{"F",4}{"GM",5}{"IM",5}{"MS",5}{"P",5}");
        int place = 1;
        foreach (StandingRow row in rows)
        {
            Console.WriteLine($"{place,-3}{row.TeamName,-20}{row.Played,4}{row.Won,4}{row.Drawn,4}{row.Lost,4}" +
                              $"{row.GoalsFor,5}{row.GoalsAgainst,5}{_formatter.FormatGoalDifference(row.GoalDifference),5}{row.Points,5}");
            place++;
        }
        Console.WriteLine("S = spelade, V = vinster, O = oavgjorda, F = förluster, GM = gjorda mål, IM = insläppta mål, MS = målskillnad, P = poäng");
    }
}
