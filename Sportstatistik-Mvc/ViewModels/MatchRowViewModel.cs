namespace Sportstatistik.ViewModels;

/// <summary>
/// En match som den ska visas, med lagens namn uppslagna och resultatet redan formaterat.
/// </summary>
public class MatchRowViewModel
{
    public int Id { get; set; }
    public string HomeTeam { get; set; } = "";
    public string AwayTeam { get; set; } = "";
    public string Result { get; set; } = "";

    public string Title => $"{HomeTeam} mot {AwayTeam}";

    // Du behöver bygga ut detta mer: datum och anteckning när modellen Match har de fälten.
}
