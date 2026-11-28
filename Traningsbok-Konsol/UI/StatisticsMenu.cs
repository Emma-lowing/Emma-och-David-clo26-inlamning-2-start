using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.UI;

/// <summary>
/// Menyval för veckoöversikt och statistik för en övning. Logiken ligger i IStatisticsService.
/// </summary>
public class StatisticsMenu
{
    private readonly AthleteSession _session;
    private readonly IStatisticsService _statistics;
    private readonly IWorkoutFormatter _formatter;

    public StatisticsMenu(IStatisticsService statistics,
     IWorkoutFormatter formatter,
     AthleteSession session)
    {
        _statistics = statistics;
        _formatter = formatter;
        _session = session;
    }

    public void ShowExerciseStats()
    {
        Console.WriteLine("\n=== Statistik för en övning ===");
        int athleteId = _session.RequireActiveAthlete();
        string name = ConsoleInput.ReadRequiredText("Övning");
        ExerciseStats stats = _statistics.GetExerciseStats(athleteId, name);

        Console.WriteLine();
        Console.WriteLine(stats.ExerciseName);
        Console.WriteLine($"  Högsta vikt: {_formatter.FormatWeight(stats.MaxWeight)}");
        Console.WriteLine($"  Totalt antal set: {stats.TotalSets}");
    }

    public void ShowWeekOverview()
    {
        Console.WriteLine("\n=== Veckoöversikt ===");
        int athleteId = _session.RequireActiveAthlete();
        int year = ConsoleInput.ReadInt("År", min: 2000, max: 2100);
        int week = ConsoleInput.ReadInt("Veckonummer", min: 1, max: 53);
        WeekOverview overview = _statistics.GetWeekOverview(athleteId, year, week);

        Console.WriteLine();
        Console.WriteLine($"Vecka {overview.Week}, {overview.Year} (måndag till söndag)");
        Console.WriteLine($"  Antal pass: {overview.WorkoutCount}");
        Console.WriteLine($"  Total träningstid: {_formatter.FormatDuration(overview.TotalMinutes)}");

        foreach (TrainingTypeMinutes row in overview.MinutesPerType)
        {
            string goal = row.GoalMinutes is int goalMinutes
                ? $" (veckomål {_formatter.FormatDuration(goalMinutes)})"
                : "";
            Console.WriteLine($"  {row.TrainingTypeName}: {_formatter.FormatDuration(row.Minutes)}{goal}");
        }
    }
}
