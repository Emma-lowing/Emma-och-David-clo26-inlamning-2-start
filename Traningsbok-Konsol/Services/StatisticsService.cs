using System.Globalization;
using Traningsbok.Extensions;
using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Ska byggas av er. Båda metoderna visar demodata så att sidorna går att titta på.
/// </summary>
public class StatisticsService : IStatisticsService
{
    private readonly IWorkoutRepository _repository;

    public StatisticsService(IWorkoutRepository repository)
    {
        _repository = repository;
    }
    // ISOWeek används för att hantera veckor och årsskiften korrekt.
    // Veckan räknas från måndag till söndag.
    public WeekOverview GetWeekOverview(int athleteId, int year, int weekNumber)
    {
        if (weekNumber < 1 || weekNumber > ISOWeek.GetWeeksInYear(year))
        {
            throw new ArgumentException("Veckonumret är ogiltigt för det valda året.");
        }

        DateOnly date = DateOnly.FromDateTime(
            ISOWeek.ToDateTime(year, weekNumber, DayOfWeek.Monday));

        DateOnly start = date.StartOfWeek();
        DateOnly end = date.EndOfWeek();

        var workouts = _repository.GetAllWorkouts()
            .Where(w => w.AthleteId == athleteId
                && w.Date >= start
                && w.Date <= end)
            .ToList();

        var minutesPerType = workouts
            .GroupBy(w => w.TrainingTypeId)
            .Select(group => new TrainingTypeMinutes
            {
                TrainingTypeName = _repository
                    .GetTrainingType(group.Key)?.Name ?? "Okänd",

                Minutes = group.Sum(w => w.TimeInMinutes),

                GoalMinutes = _repository
                    .GetWeeklyGoal(athleteId, group.Key)?.TargetMinutes
            })
            .ToList();

        return new WeekOverview
        {
            Year = year,
            Week = weekNumber,
            WorkoutCount = workouts.Count,
            TotalMinutes = workouts.Sum(w => w.TimeInMinutes),
            MinutesPerType = minutesPerType
        };
    }

    public ExerciseStats GetExerciseStats(int athleteId, string exerciseName) => new()
    {
        ExerciseName = exerciseName,
        MaxWeight = 65m,
        TotalSets = 7,
    };
}
