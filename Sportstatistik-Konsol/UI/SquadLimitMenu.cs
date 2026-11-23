using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.UI;

/// <summary>
/// Menyval för truppgränser per lag och säsong. Logiken ligger i ISquadLimitService.
/// </summary>
public class SquadLimitMenu
{
    private readonly ISquadLimitService _squadLimits;
    private readonly ILeagueRepository _repository;

    public SquadLimitMenu(ISquadLimitService squadLimits, ILeagueRepository repository)
    {
        _squadLimits = squadLimits;
        _repository = repository;
    }

    public void Run()
    {
        new Menu("Truppgränser")
            .Add("1", "Visa truppgränser", ShowLimits)
            .Add("2", "Sätt truppgräns för ett lag", SetLimit)
            .Run();
    }

    private void ShowLimits()
    {
        int season = AskForSeason();
        List<SquadLimit> limits = _squadLimits.GetLimits(season);

        Console.WriteLine($"Truppgränser för säsong {season}");
        foreach (Team team in _repository.GetAllTeams())
        {
            SquadLimit? limit = limits.FirstOrDefault(l => l.TeamId == team.Id);
            int players = _repository.GetPlayers(team.Id).Count;
            string max = limit is null ? "ingen gräns" : $"högst {limit.MaxPlayers}";
            Console.WriteLine($"  {team.Name}: {players} spelare, {max}");
        }
    }

    private void SetLimit()
    {
        foreach (Team team in _repository.GetAllTeams())
        {
            Console.WriteLine($"{team.Id}. {team.Name}");
        }

        int teamId = ConsoleInput.ReadInt("Lagets nummer", min: 1);
        if (_repository.GetTeam(teamId) is null)
        {
            ConsoleInput.WriteWarning($"Det finns inget lag med nummer {teamId}.");
            return;
        }

        int season = AskForSeason();
        int max = ConsoleInput.ReadInt("Högst antal spelare", min: 1, max: 100);

        _squadLimits.SetLimit(teamId, season, max);
        Console.WriteLine("Truppgränsen är sparad.");
    }

    private static int AskForSeason() => ConsoleInput.ReadInt("Säsong (år)", min: 2000, max: 2100);
}
