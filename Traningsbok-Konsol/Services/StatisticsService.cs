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
    // DEMO: samma påhittade siffror varje vecka. Ersätt med en översikt som räknas fram ur veckans pass.
    // ISOWeek räknar veckonummer med måndag som första dag (önskemål 2):
    // ett pass söndag 15 november 2026 hör till vecka 46, inte vecka 47.
    public WeekOverview GetWeekOverview(int athleteId, int year, int weekNumber) => new()
    {
        Year = year,
        Week = weekNumber,
        WorkoutCount = 4,
        TotalMinutes = 166,
        MinutesPerType =
        [
            new TrainingTypeMinutes { TrainingTypeName = "Styrka", Minutes = 65, GoalMinutes = 120 },
            new TrainingTypeMinutes { TrainingTypeName = "Kondition", Minutes = 81, GoalMinutes = 100 },
            new TrainingTypeMinutes { TrainingTypeName = "Rörlighet", Minutes = 20 },
        ],
    };

    // DEMO: samma siffror oavsett vilken övning som efterfrågas.
    // Ersätt med högsta vikt och summan av alla set för övningen, i alla pass.
    public ExerciseStats GetExerciseStats(int athleteId, string exerciseName) => new()
    {
        ExerciseName = exerciseName,
        MaxWeight = 65m,
        TotalSets = 7,
    };
}
