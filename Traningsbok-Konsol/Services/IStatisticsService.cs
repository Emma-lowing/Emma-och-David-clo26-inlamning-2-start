using Traningsbok.Models;

namespace Traningsbok.Services;

public interface IStatisticsService
{
    WeekOverview GetWeekOverview(int athleteId, int year, int weekNumber);

    ExerciseStats GetExerciseStats(int athleteId, string exerciseName);
}