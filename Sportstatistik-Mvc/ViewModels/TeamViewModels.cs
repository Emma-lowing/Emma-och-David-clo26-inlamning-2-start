using Sportstatistik.Models;

namespace Sportstatistik.ViewModels;

/// <summary>
/// Ett lag i listan, med antalet spelare.
/// </summary>
public class TeamSummaryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int PlayerCount { get; set; }
}

/// <summary>
/// Ett lag med sina spelare.
/// </summary>
public class TeamDetailsViewModel
{
    public Team Team { get; set; } = new();
    public List<Player> Players { get; set; } = new();
}
