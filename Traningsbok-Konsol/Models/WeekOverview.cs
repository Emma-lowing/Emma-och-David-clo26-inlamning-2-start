namespace Traningsbok.Models;

/// <summary>
/// Veckoöversikten för en vecka: antal pass, total träningstid och tid per träningstyp.
/// En vecka börjar alltid på måndag (önskemål 2).
/// </summary>
public class WeekOverview
{
    public int Year { get; set; }
    public int Week { get; set; }
    public int WorkoutCount { get; set; }
    public int TotalMinutes { get; set; }
    public List<TrainingTypeMinutes> MinutesPerType { get; set; } = new();
}

/// <summary>
/// En rad i veckoöversikten: hur många minuter en träningstyp har tränats, och veckomålet om det finns.
/// </summary>
public class TrainingTypeMinutes
{
    public string TrainingTypeName { get; set; } = "";
    public int Minutes { get; set; }
    public int? GoalMinutes { get; set; }
}
