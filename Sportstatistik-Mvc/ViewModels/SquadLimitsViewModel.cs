namespace Sportstatistik.ViewModels;

/// <summary>
/// Truppgränserna för en vald säsong.
/// </summary>
public class SquadLimitsViewModel
{
    public int Season { get; set; }
    public List<SquadLimitRowViewModel> Rows { get; set; } = new();
}

/// <summary>
/// Ett lag, hur många spelare det har och dess truppgräns. MaxPlayers är null om laget saknar gräns.
/// </summary>
public class SquadLimitRowViewModel
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = "";
    public int PlayerCount { get; set; }
    public int? MaxPlayers { get; set; }
}
