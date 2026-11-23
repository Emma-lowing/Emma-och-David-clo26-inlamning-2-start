using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Kontraktet mellan gränssnittet och datalagret. Sidorna känner bara till det här interfacet,
/// så ni kan byta InMemoryWorkoutRepository mot en riktig databas utan att röra sidorna.
/// </summary>
public interface IWorkoutRepository
{
    List<Workout> GetAllWorkouts();
    Workout? GetWorkout(int id);
    void AddWorkout(Workout workout);
    void DeleteWorkout(int id);
    void AddExercise(int workoutId, Exercise exercise);

    List<Athlete> GetAllAthletes();
    void AddAthlete(Athlete athlete);

    // Du behöver bygga ut detta mer: metoder för träningstyper och veckomål när ni sparar dem i databasen.
}
