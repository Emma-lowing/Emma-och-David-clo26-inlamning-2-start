using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Låtsasdatabas: allt ligger i listor i minnet och försvinner när programmet stängs.
/// Den finns bara för att menyn ska gå att köra från dag ett.
/// Ersätt den med en egen klass som implementerar IWorkoutRepository mot er databas,
/// och byt sedan en enda rad i Program.cs.
/// </summary>
public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly List<Workout> _workouts = new();
    private readonly List<Athlete> _athletes = new();
    private int _nextWorkoutId = 1;
    private int _nextExerciseId = 1;
    private int _nextAthleteId = 1;

    public InMemoryWorkoutRepository()
    {
        Seed();
    }

    public List<Workout> GetAllWorkouts() => _workouts.OrderBy(w => w.Date).ToList();

    public Workout? GetWorkout(int id) => _workouts.FirstOrDefault(w => w.Id == id);

    public void AddWorkout(Workout workout)
    {
        workout.Id = _nextWorkoutId++;
        foreach (Exercise exercise in workout.Exercises)
        {
            exercise.Id = _nextExerciseId++;
        }
        _workouts.Add(workout);
    }

    public void DeleteWorkout(int id) => _workouts.RemoveAll(w => w.Id == id);

    public void AddExercise(int workoutId, Exercise exercise)
    {
        Workout workout = GetWorkout(workoutId)
            ?? throw new ArgumentException($"Det finns inget pass med nummer {workoutId}.");

        exercise.Id = _nextExerciseId++;
        workout.Exercises.Add(exercise);
    }

    public List<Athlete> GetAllAthletes() => _athletes.ToList();

    public void AddAthlete(Athlete athlete)
    {
        // Du behöver bygga ut detta mer: beställningen säger att programmet ska klara 1–5 utövare.
        athlete.Id = _nextAthleteId++;
        _athletes.Add(athlete);
    }

    // Exempeldata, så att det finns något att titta på direkt.
    // Er riktiga databas ska seeda sig själv på samma sätt vid första start, om den är tom.
    // Här passar till exempel mönstret Builder: ett pass med flera övningar blir lättare att läsa om det
    // byggs steg för steg, till exempel .WithExercise("Knäböj", 3, 8, 80).
    private void Seed()
    {
        AddAthlete(new Athlete { FirstName = "Kim", LastName = "Exempelsson" });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 12),
            Type = "Styrka",
            Exercises =
            {
                new Exercise { Name = "Knäböj", Sets = 3, Reps = 8, Weight = 62.5m },
                new Exercise { Name = "Marklyft", Sets = 3, Reps = 5, Weight = 80m },
            }
        });

        // En söndag: passet hör till vecka 46, inte vecka 47 (önskemål 2).
        AddWorkout(new Workout { Date = new DateOnly(2026, 11, 15), Type = "Kondition" });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 18),
            Type = "Styrka",
            Exercises =
            {
                new Exercise { Name = "Knäböj", Sets = 4, Reps = 6, Weight = 65m },
                new Exercise { Name = "Bänkpress", Sets = 3, Reps = 8, Weight = 45m },
            }
        });

        AddWorkout(new Workout { Date = new DateOnly(2026, 11, 20), Type = "Rörlighet" });
    }
}
