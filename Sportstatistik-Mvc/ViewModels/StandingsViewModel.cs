namespace Sportstatistik.ViewModels;

/// <summary>
/// Tabellen för en vald säsong.
/// </summary>
public class StandingsViewModel
{
    public int Season { get; set; }
    public List<StandingLineViewModel> Lines { get; set; } = new();
}

/// <summary>
/// En rad i tabellen, med målskillnaden redan formaterad.
/// </summary>
public class StandingLineViewModel
{
    public int Place { get; set; }
    public string TeamName { get; set; } = "";
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public string GoalDifference { get; set; } = "";
    public int Points { get; set; }
}
