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
    private readonly IWorkoutRepository _repository;
    private readonly ITrainingTypeService _trainingTypes;
    private readonly IGoalNotifier _goals;
    private readonly IWorkoutFormatter _formatter;

    public WorkoutMenu(IWorkoutRepository repository, ITrainingTypeService trainingTypes,
        IGoalNotifier goals, IWorkoutFormatter formatter)
    {
        _repository = repository;
        _trainingTypes = trainingTypes;
        _goals = goals;
        _formatter = formatter;
    }

    public void ListWorkouts()
    {
        List<Workout> workouts = _repository.GetAllWorkouts();
        if (workouts.Count == 0)
        {
            Console.WriteLine("Det finns inga pass än.");
            return;
        }

        foreach (Workout workout in workouts)
        {
            // Du behöver bygga ut detta mer: visa längd och utövare när modellen Workout har dem.
            Console.WriteLine($"{workout.Id}. {workout.Date:yyyy-MM-dd} {workout.Type} ({workout.Exercises.Count} övningar)");
        }
    }

    public void AddWorkout()
    {
        string typeNames = string.Join(", ", _trainingTypes.GetAll().Select(t => t.Name));
        Console.WriteLine($"Träningstyper: {typeNames}");

        var workout = new Workout
        {
            Date = ConsoleInput.ReadDate("Datum"),
            // Tom träningstyp ger ett meddelande och en ny fråga.
            // Du behöver bygga ut detta mer: kontrollera att träningstypen finns.
            Type = ConsoleInput.ReadRequiredText("Träningstyp"),
        };

        // Du behöver bygga ut detta mer: fråga efter längd i minuter, anteckning och utövare
        // när modellen Workout har de fälten.

        _repository.AddWorkout(workout);
        Console.WriteLine($"Passet den {workout.Date:yyyy-MM-dd} är sparat.");

        // Funktion 6: meddelande när träningstypen når 80 % eller 100 % av veckomålet.
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
        Console.WriteLine($"{workout.Type}, {workout.Date:dddd d MMMM yyyy}");
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
        // Fälten följer modellen Athlete. Tar ni bort ett fält ur modellen, ta bort raden här också.
        var athlete = new Athlete
        {
            FirstName = ConsoleInput.ReadRequiredText("Förnamn"),
            LastName = ConsoleInput.ReadText("Efternamn"),
            BirthDate = ConsoleInput.ReadDate("Födelsedatum"),
            HairColor = ConsoleInput.ReadText("Hårfärg"),
            HeightCm = ConsoleInput.ReadInt("Längd i cm", min: 0, max: 300),
            Religion = ConsoleInput.ReadText("Religion"),
        };

        _repository.AddAthlete(athlete);
        Console.WriteLine($"Utövaren {athlete.FirstName} är tillagd.");
    }

    private Workout? AskForWorkout()
    {
        ListWorkouts();
        int id = ConsoleInput.ReadInt("Passets nummer", min: 1);
        Workout? workout = _repository.GetWorkout(id);

        if (workout is null)
        {
            ConsoleInput.WriteWarning($"Det finns inget pass med nummer {id}.");
        }
        return workout;
    }
}
