using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.UI;

/// <summary>
/// Menyval för pass, övningar och utövare. Klassen sköter bara inmatning och utskrift;
/// all lagring går via IWorkoutRepository, meddelanden om veckomål via IGoalNotifier
/// och formatering via IWorkoutFormatter.
/// </summary>
public class WorkoutMenu
{
    private readonly AthleteSession _session;
    private readonly IWorkoutRepository _repository;
    private readonly ITrainingTypeService _trainingTypes;
    private readonly IGoalNotifier _goals;
    private readonly IWorkoutFormatter _formatter;

    public WorkoutMenu(IWorkoutRepository repository,
     ITrainingTypeService trainingTypes,
     IGoalNotifier goals,
     IWorkoutFormatter formatter,
     AthleteSession session)
    {
        _repository = repository;
        _trainingTypes = trainingTypes;
        _goals = goals;
        _formatter = formatter;
        _session = session;
    }

    public void ListWorkouts()
    {
        int athleteId = _session.RequireActiveAthlete();

        List<Workout> workouts = _repository.GetAllWorkouts()
            .Where(w => w.AthleteId == athleteId)
            .ToList();
        if (workouts.Count == 0)
        {
            Console.WriteLine("Det finns inga pass än.");
            return;
        }

        foreach(Workout workout in workouts)
{
            string typeName = _repository
                .GetTrainingType(workout.TrainingTypeId)?.Name
                ?? "Okänd träningstyp";

            Console.WriteLine($"{workout.Id}. {workout.Date:yyyy-MM-dd} {typeName} ({workout.Exercises.Count} övningar)");
        }
    }

    public void AddWorkout()
    {
        int athleteId = _session.RequireActiveAthlete();

        foreach (TrainingType type in _repository.GetAllTrainingTypes())
        {
            Console.WriteLine($"{type.Id}. {type.Name}");
        }

        int typeId = ConsoleInput.ReadInt("Träningstypens nummer", min: 1);

        if (_repository.GetTrainingType(typeId) is null)
        {
            throw new ArgumentException("Träningstypen finns inte.");
        }

        var workout = new Workout
        {
            AthleteId = athleteId,
            TrainingTypeId = typeId,
            Date = ConsoleInput.ReadDate("Datum"),
            TimeInMinutes = ConsoleInput.ReadInt("Längd i minuter", min: 1),
            Note = ConsoleInput.ReadText("Anteckning (valfritt)")
        };

        _repository.AddWorkout(workout);

        Console.WriteLine($"Passet den {workout.Date:yyyy-MM-dd} är sparat.");

        foreach (string message in _goals.CheckGoals(workout))
        {
            ConsoleInput.WriteWarning(message);
        }
    }

    public void AddExercise()
    {
        Workout? workout = AskForWorkout();
        if (workout is null)
        {
            return;
        }

        // Ogiltig vikt, till exempel "abc", ger ett meddelande och en ny fråga.
        // Ingen övning sparas förrän alla fält går att tolka.
        var exercise = new Exercise
        {
            Name = ConsoleInput.ReadRequiredText("Övning"),
            Sets = ConsoleInput.ReadInt("Antal set", min: 1, max: 100),
            Reps = ConsoleInput.ReadInt("Antal reps", min: 1, max: 1000),
            Weight = ConsoleInput.ReadDecimal("Vikt i kg", min: 0),
        };

        _repository.AddExercise(workout.Id, exercise);
        Console.WriteLine($"Övningen \"{exercise.Name}\" är tillagd.");
    }

    public void ShowWorkout()
    {
        Workout? workout = AskForWorkout();
        if (workout is null)
        {
            return;
        }

        Console.WriteLine();
        string typeName = _repository
     .GetTrainingType(workout.TrainingTypeId)?.Name
     ?? "Okänd träningstyp";

        Console.WriteLine($"{typeName}, {workout.Date:dddd d MMMM yyyy}");
        if (workout.Exercises.Count == 0)
        {
            Console.WriteLine("  Inga övningar.");
        }
        foreach (Exercise exercise in workout.Exercises)
        {
            Console.WriteLine($"  {exercise.Name}: {exercise.Sets} set × {exercise.Reps} reps, {_formatter.FormatWeight(exercise.Weight)}");
        }

        // Du behöver bygga ut detta mer: visa längd, anteckning och utövare.
    }

    public void DeleteWorkout()
    {
        Workout? workout = AskForWorkout();
        if (workout is null)
        {
            return;
        }

        // Önskemål 5: bekräfta innan något tas bort.
        if (!ConsoleInput.Confirm($"Är du säker på att du vill ta bort passet den {workout.Date:yyyy-MM-dd}?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _repository.DeleteWorkout(workout.Id);
        Console.WriteLine("Passet är borttaget.");
    }

    public void ListAthletes()
    {
        foreach (Athlete athlete in _repository.GetAllAthletes())
        {
            Console.WriteLine($"{athlete.Id}. {athlete.FirstName} {athlete.LastName}");
        }
    }

    public void AddAthlete()
    {
        var athlete = new Athlete
        {
            FirstName = ConsoleInput.ReadRequiredText("Förnamn"),
            LastName = ConsoleInput.ReadText("Efternamn")
        };

        _repository.AddAthlete(athlete);

        Console.WriteLine($"Utövaren {athlete.FirstName} är tillagd.");
    }

    private Workout? AskForWorkout()
    {
        ListWorkouts();
        int id = ConsoleInput.ReadInt("Passets nummer", min: 1);
        Workout? workout = _repository.GetWorkout(id);

        if (workout is null ||
    workout.AthleteId != _session.RequireActiveAthlete())
        {
            ConsoleInput.WriteWarning("Passet finns inte för den aktiva utövaren.");
            return null;
        }
        return workout;
    }
}
