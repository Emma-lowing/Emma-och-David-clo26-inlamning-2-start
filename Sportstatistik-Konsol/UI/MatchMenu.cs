using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.UI;

/// <summary>
/// Menyval för matcher. Klassen sköter bara inmatning och utskrift;
/// all lagring går via ILeagueRepository och all formatering via IResultFormatter.
/// </summary>
public class MatchMenu
{
    private readonly ILeagueRepository _repository;
    private readonly IResultFormatter _formatter;

    public MatchMenu(ILeagueRepository repository, IResultFormatter formatter)
    {
        _repository = repository;
        _formatter = formatter;
    }

    public void RegisterMatch()
    {
        List<Team> teams = _repository.GetAllTeams();
        if (teams.Count < 2)
        {
            Console.WriteLine("Det behövs minst två lag för att registrera en match.");
            return;
        }

        foreach (Team team in teams)
        {
            Console.WriteLine($"{team.Id}. {team.Name}");
        }

        Team? home = AskForTeam("Hemmalagets nummer");
        if (home is null)
        {
            return;
        }
        Team? away = AskForTeam("Bortalagets nummer");
        if (away is null)
        {
            return;
        }

        // Ingen kontroll av att ett lag möter sig självt här: den regeln hör hemma i repository eller service.
        var match = new Match
        {
            HomeTeamId = home.Id,
            AwayTeamId = away.Id,
            HomeGoals = ConsoleInput.ReadInt("Mål hemma", min: 0, max: 99),
            AwayGoals = ConsoleInput.ReadInt("Mål borta", min: 0, max: 99),
        };

        // Du behöver bygga ut detta mer: fråga efter datum (ConsoleInput.ReadDate) och en valfri
        // anteckning när modellen Match har de fälten.

        _repository.AddMatch(match);
        Console.WriteLine($"Matchen är sparad: {home.Name} mot {away.Name}, {_formatter.FormatResult(match.HomeGoals, match.AwayGoals)}");
    }

    public void ListMatches()
    {
        List<Match> matches = _repository.GetAllMatches();
        if (matches.Count == 0)
        {
            Console.WriteLine("Det finns inga matcher än.");
            return;
        }

        foreach (Match match in matches)
        {
            // Du behöver bygga ut detta mer: visa datum och anteckning, och sortera på datum.
            Console.WriteLine($"{match.Id}. {TeamName(match.HomeTeamId)} mot {TeamName(match.AwayTeamId)}, " +
                              $"{_formatter.FormatResult(match.HomeGoals, match.AwayGoals)}");
        }
    }

    public void DeleteMatch()
    {
        ListMatches();
        int id = ConsoleInput.ReadInt("Matchens nummer", min: 1);
        Match? match = _repository.GetMatch(id);
        if (match is null)
        {
            ConsoleInput.WriteWarning($"Det finns ingen match med nummer {id}.");
            return;
        }

        // Önskemål 5: bekräfta innan något tas bort. Svaret "n" lämnar allt oförändrat.
        if (!ConsoleInput.Confirm("Är du säker?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _repository.DeleteMatch(match.Id);
        Console.WriteLine("Matchen är borttagen.");
    }

    private Team? AskForTeam(string prompt)
    {
        int id = ConsoleInput.ReadInt(prompt, min: 1);
        Team? team = _repository.GetTeam(id);

        if (team is null)
        {
            ConsoleInput.WriteWarning($"Det finns inget lag med nummer {id}.");
        }
        return team;
    }

    private string TeamName(int teamId) => _repository.GetTeam(teamId)?.Name ?? "(okänt lag)";
}
