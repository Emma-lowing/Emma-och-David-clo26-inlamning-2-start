using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Veckoöversikt och statistik för en övning. En vecka börjar alltid på måndag (önskemål 2).
/// </summary>
public interface IStatisticsService
{
    /// <summary>Antal pass, total träningstid och träningstid per träningstyp för veckan (funktion 4).</summary>
    WeekOverview GetWeekOverview(int year, int weekNumber);

    /// <summary>Högsta vikt och totalt antal set för en övning, i alla pass (funktion 7).</summary>
    ExerciseStats GetExerciseStats(string exerciseName);
}
