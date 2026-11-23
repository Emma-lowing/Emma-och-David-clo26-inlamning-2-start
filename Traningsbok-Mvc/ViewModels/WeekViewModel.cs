namespace Traningsbok.ViewModels;

/// <summary>
/// Veckoöversikten för en vald vecka, med tiderna redan formaterade.
/// </summary>
public class WeekViewModel
{
    public int Year { get; set; }
    public int Week { get; set; }
    public int WorkoutCount { get; set; }
    public string TotalTime { get; set; } = "";

    /// <summary>En rad per träningstyp. Goal är null om träningstypen saknar veckomål.</summary>
    public List<(string TrainingType, string Time, string? Goal)> Rows { get; set; } = new();
}
