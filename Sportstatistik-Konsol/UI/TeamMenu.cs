using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.UI;

/// <summary>
/// Menyval för lag och spelare. Klassen sköter bara inmatning och utskrift;
/// all lagring går via ILeagueRepository, och truppgränsen via ISquadLimitService.
/// </summary>
public class TeamMenu
{
    private readonly ILeagueRepository _repository;
    private readonly IPositionService _positions;
    private readonly ISquadLimitService _squadLimits;

    public TeamMenu(ILeagueRepository repository, IPositionService positions, ISquadLimitService squadLimits)
    {
        _repository = repository;
        _positions = positions;
        _squadLimits = squadLimits;
    }

    public void ListTeams()
    {
        List<Team> teams = _repository.GetAllTeams();
        if (teams.Count == 0)
        {
            Console.WriteLine("Det finns inga lag än.");
            return;
        }

        foreach (Team team in teams)
        {
            Console.WriteLine($"{team.Id}. {team.Name} ({_repository.GetPlayers(team.Id).Count} spelare)");
        }
    }

    public void AddTeam()
    {
        var team = new Team
        {
            Name = ConsoleInput.ReadRequiredText("Lagets namn"),
        };

        _repository.AddTeam(team);
        Console.WriteLine($"Laget \"{team.Name}\" är tillagt.");
    }

    public void ListPlayers()
    {
        Team? team = AskForTeam("Lagets nummer");
        if (team is null)
        {
            return;
        }

        List<Player> players = _repository.GetPlayers(team.Id);
        Console.WriteLine($"{team.Name}, {players.Count} spelare");
        if (players.Count == 0)
        {
            Console.WriteLine("  Laget har inga spelare än.");
            return;
        }

        foreach (Player player in players)
        {
            Console.WriteLine($"  {$"{player.FirstName} {player.LastName}".Trim()}, {player.Position}");
        }
    }

    public void AddPlayer()
    {
        Team? team = AskForTeam("Lagets nummer");
        if (team is null)
        {
            return;
        }

        string position = AskForPosition();

        // Fälten följer modellen Player. Tar ni bort ett fält ur modellen, ta bort raden här också.
        var player = new Player
        {
            TeamId = team.Id,
            Position = position,
            FirstName = ConsoleInput.ReadRequiredText("Förnamn"),
            LastName = ConsoleInput.ReadText("Efternamn"),
            BirthDate = ConsoleInput.ReadDate("Födelsedatum"),
            HairColor = ConsoleInput.ReadText("Hårfärg"),
            HeightCm = ConsoleInput.ReadInt("Längd i cm", min: 0, max: 300),
            Religion = ConsoleInput.ReadText("Religion"),
        };

        _repository.AddPlayer(player);
        Console.WriteLine($"{player.FirstName} är tillagd i {team.Name}.");

        // Funktion 6: varning vid 80 % och 100 % av truppgränsen. Säsongen är innevarande år.
        List<string> warnings = _squadLimits.CheckSquadLimit(team.Id, DateTime.Today.Year);
        foreach (string warning in warnings)
        {
            ConsoleInput.WriteWarning(warning);
        }
    }

    private Team? AskForTeam(string prompt)
    {
        ListTeams();
        int id = ConsoleInput.ReadInt(prompt, min: 1);
        Team? team = _repository.GetTeam(id);

        if (team is null)
        {
            ConsoleInput.WriteWarning($"Det finns inget lag med nummer {id}.");
        }
        return team;
    }

    private string AskForPosition()
    {
        List<Position> positions = _positions.GetAll();
        foreach (Position position in positions)
        {
            Console.WriteLine($"{position.Id}. {position.Name}");
        }

        while (true)
        {
            int id = ConsoleInput.ReadInt("Positionens nummer", min: 1);
            Position? chosen = positions.FirstOrDefault(p => p.Id == id);
            if (chosen is not null)
            {
                return chosen.Name;
            }
            ConsoleInput.WriteWarning($"Det finns ingen position med nummer {id}.");
        }
    }
}
