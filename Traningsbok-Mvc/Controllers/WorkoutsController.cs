using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Traningsbok.Models;
using Traningsbok.Services;
using Traningsbok.ViewModels;

namespace Traningsbok.Controllers;

/// <summary>
/// Sidor för pass och övningar. Controllern sköter bara formulär och sidor;
/// lagring går via IWorkoutRepository, meddelanden om veckomål via IGoalNotifier
/// och formatering via IWorkoutFormatter.
/// </summary>
public class WorkoutsController : AppController
{
    private readonly IWorkoutRepository _repository;
    private readonly ITrainingTypeService _trainingTypes;
    private readonly IGoalNotifier _goals;
    private readonly IWorkoutFormatter _formatter;

    public WorkoutsController(IWorkoutRepository repository, ITrainingTypeService trainingTypes,
        IGoalNotifier goals, IWorkoutFormatter formatter)
    {
        _repository = repository;
        _trainingTypes = trainingTypes;
        _goals = goals;
        _formatter = formatter;
    }

    public IActionResult Index() => Run(() => View(_repository.GetAllWorkouts()));

    public IActionResult Create() => Run(() =>
    {
        ViewData["TrainingTypes"] = TrainingTypeOptions();
        return View(new WorkoutFormModel { Date = DateOnly.FromDateTime(DateTime.Today) });
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(WorkoutFormModel form) => Run(() =>
    {
        if (!ModelState.IsValid)
        {
            ViewData["TrainingTypes"] = TrainingTypeOptions();
            return View(form);
        }

        try
        {
            Workout workout = form.ToWorkout();
            _repository.AddWorkout(workout);
            TempData["Message"] = $"Passet den {workout.Date:yyyy-MM-dd} är sparat.";
            ShowGoalMessages(workout);
            return RedirectToAction(nameof(Details), new { id = workout.Id });
        }
        catch (ArgumentException ex)
        {
            // Felaktig inmatning som upptäcks i era services visas i formuläret.
            ModelState.AddModelError("", ex.Message);
            ViewData["TrainingTypes"] = TrainingTypeOptions();
            return View(form);
        }
    });

    public IActionResult Details(int id) => Run(() =>
    {
        Workout? workout = _repository.GetWorkout(id);
        if (workout is null)
        {
            return NotFound();
        }

        return View(new WorkoutDetailsViewModel
        {
            Id = workout.Id,
            Date = workout.Date,
            Type = workout.Type,
            ExerciseLines = workout.Exercises
                .Select(e => $"{e.Name}: {e.Sets} set × {e.Reps} reps, {_formatter.FormatWeight(e.Weight)}")
                .ToList(),
        });
    });

    public IActionResult AddExercise(int id) => Run(() =>
    {
        Workout? workout = _repository.GetWorkout(id);
        if (workout is null)
        {
            return NotFound();
        }

        ViewData["Workout"] = workout;
        return View(new ExerciseFormModel());
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddExercise(int id, ExerciseFormModel form) => Run(() =>
    {
        Workout? workout = _repository.GetWorkout(id);
        if (workout is null)
        {
            return NotFound();
        }

        // Story 6: ogiltig vikt, till exempel "abc", ger ett felmeddelande och ingen övning sparas.
        if (!ModelState.IsValid)
        {
            ViewData["Workout"] = workout;
            return View(form);
        }

        try
        {
            Exercise exercise = form.ToExercise();
            _repository.AddExercise(id, exercise);
            TempData["Message"] = $"Övningen \"{exercise.Name}\" är tillagd.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["Workout"] = workout;
            return View(form);
        }
    });

    // Önskemål 5: först en sida som frågar "Är du säker?", sedan tas passet bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        Workout? workout = _repository.GetWorkout(id);
        return workout is null ? NotFound() : View(workout);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _repository.DeleteWorkout(id), "Passet är borttaget.", nameof(Index));

    // Funktion 6: meddelande när träningstypen når 80 % eller 100 % av veckomålet.
    // Passet är redan sparat, så "Inte implementerat ännu" visas här som en varning
    // i stället för som en egen sida.
    private void ShowGoalMessages(Workout workout)
    {
        try
        {
            List<string> messages = _goals.CheckGoals(workout);
            if (messages.Count > 0)
            {
                TempData["Warning"] = string.Join(" ", messages);
            }
        }
        catch (NotImplementedException)
        {
            TempData["Warning"] = "Meddelanden vid veckomålet är inte implementerade ännu: GoalNotifier kastar NotImplementedException.";
        }
    }

    private List<SelectListItem> TrainingTypeOptions() =>
        _trainingTypes.GetAll()
            .Select(t => new SelectListItem(t.Name, t.Name))
            .ToList();
}
