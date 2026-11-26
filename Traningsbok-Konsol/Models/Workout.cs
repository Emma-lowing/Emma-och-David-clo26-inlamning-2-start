using Microsoft.VisualBasic;

namespace Traningsbok.Models;

/// <summary>
/// Ett träningspass. Minimal modell: bara det som README:n kräver.
/// </summary>
public class Workout
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TrainingTypeId { get; set; }
    public List<Exercise> Exercises { get; set; } = new();
    public int TimeInMinutes { get; set; }
    public string? Note { get; set; }
    public int AthleteId { get; set; }



    // Du behöver bygga ut detta mer.
    // Beställningen kräver längd i minuter, en valfri anteckning och vilken utövare passet tillhör.
    // Fundera också på om Type ska vara en koppling till TrainingType i stället för en text.
    // När ni lägger till fälten: uppdatera även UI/WorkoutMenu.cs så att användaren kan fylla i dem.
}
